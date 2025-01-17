import { Component , AfterViewInit } from '@angular/core';
import * as L from 'leaflet';
import * as countries from 'countries-list';
import { Geocoder, geocoders } from 'leaflet-control-geocoder';
import "leaflet/dist/leaflet.css";
//import the css
import "leaflet-control-geocoder/dist/Control.Geocoder.css";
import "leaflet.vectorgrid";
import { CustomerserviceService } from '../../service/customerservice.service';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrl: './map.component.css',
  standalone: false
})

/*
https://github.com/perliedman/leaflet-control-geocoder/blob/master/src/control.ts
reverse engineer the github repository, most of the commands are under controls

*/
export class MapComponent implements AfterViewInit {

  constructor(private customerService: CustomerserviceService) {
  }
  private savedRoutes: { [key: string]: string } = {}; // Store as routeId
  private routeLocations: { [key: string]: L.LatLng } = {}; //Stored as route coordinates
  locationSearchQuery: string = '';
  activeTab: 'location' | 'route' = 'location';
  routeSearchQuery: string = '';
  private geocoder: Geocoder | null = null;
  private tooltip: L.Popup | null = null;
  private map!: L.Map; // Explicitly define the type as L.Map

  private initMap(): void {
    //https://leafletjs.com/examples/quick-start/
    //guide will help us set up the map
    this.map = L.map('map', {
      center: [40.8282, -98.5795],
      zoom: 5
    });

    const tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 14,

      attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    });
    tiles.addTo(this.map);



    //https://www.transit.land/examples/example-map.html?apikey=Z2xK57toXiR4t1cLlMvfC4fofM4ZhmVV
    // https://transit.land/api/v2/rest/agencies?adm0_name=Mexico&apikey=Z2xK57toXiR4t1cLlMvfC4fofM4ZhmVV
    //operator_onestop_id=UPV&apikey=Z2xK57toXiR4t1cLlMvfC4fofM4ZhmVV
    //needed to install @types/leaflet.vectorgrid
    //The plugin only supported js so we need @types
    //replace the api key later when fully finished
    //https://leaflet.github.io/Leaflet.VectorGrid/vectorgrid-api-docs.html#vectorgrid-protobuf

    const mapBox = L.vectorGrid.protobuf(
      `https://transit.land/api/v2/tiles/routes/tiles/{z}/{x}/{y}.pbf?include_geometry=true&apikey=Z2xK57toXiR4t1cLlMvfC4fofM4ZhmVV`,
      {
        vectorTileLayerStyles: {
          routes: {
            weight: 3,
            color: "#ff0000",
            opacity: 1,
            lineCap: 'round',
            lineJoin: 'round'
          }
        },
        maxZoom: 14,
        interactive: true,


        getFeatureId: (f: any) => {
          const routeId = f.properties.route_id;
          return routeId;
        },

      }).addTo(this.map);


    this.tooltip = L.popup({
      closeButton: false,
      offset: L.point(0, -10),
      className: 'route-tooltip',
      autoPan: false
    });



    mapBox.on('mouseover', (e: any) => {
      if (e.layer) {
        e.layer.setStyle({
          weight: 5,
          color: '#FFD700'
        });
        const props = e.layer.properties;

        //  console.log('All properties:', props);
        // Create content to display information
        const content = `
          <div class="route-info">
            ${props.route_short_name ? `<div>Route: ${props.route_short_name}</div>` : ''}
            ${props.route_long_name ? `<div>${props.route_long_name}</div>` : ''}
            ${props.route_id ? `<div>ID: ${props.route_id}</div>` : ''}
            ${props.bbox ? `<div>Bounds: ${props.bbox}</div>` : ''}
          </div>
        `;
        //
        this.tooltip!
          .setLatLng(e.latlng)
          .setContent(content)
          .openOn(this.map);
      }

    });
    mapBox.on('mouseout', (e: any) => {
      if (e.layer) {
        e.layer.setStyle({
          weight: 3,
          color: '#ff0000'
        });
      }
      if (this.tooltip) {
        this.map.closePopup(this.tooltip);
      }
    });
    mapBox.on('mousemove', (e: any) => {
      if (this.tooltip && this.tooltip.isOpen()) {
        this.tooltip.setLatLng(e.latlng);
      }
    });
    // Attach click handler to the vector tile layer, not the map
    mapBox.on('click', (e: any) => {
      if (e.layer && e.layer.properties) {
        const props = e.layer.properties;
        this.routeLocations[props.route_id] = e.latlng;

        const saveButtonHtml = props.route_id in this.savedRoutes
          ? `<button class="unsave-route-btn" data-route-id="${props.route_id}">Unsave Route</button>`
          : `<button class="save-route-btn" data-route-id="${props.route_id}">Save Route</button>`;

        const content = `
          <div class="route-info">
            ${props.route_short_name ? `<div>Route: ${props.route_short_name}</div>` : ''}
            ${props.route_long_name ? `<div>${props.route_long_name}</div>` : ''}
            ${props.route_id ? `<div>ID: ${props.route_id}</div>` : ''}
            <div class="route-actions">
              ${saveButtonHtml}
            </div>
          </div>
        `;

        const popup = L.popup()
          .setLatLng(e.latlng)
          .setContent(content)
          .openOn(this.map);

        setTimeout(() => {
          const saveBtn = document.querySelector('.save-route-btn, .unsave-route-btn');
          if (saveBtn) {
            saveBtn.addEventListener('click', (event) => {
              const target = event.target as HTMLButtonElement;
              const routeId = target.getAttribute('data-route-id');
              if (routeId && this.routeLocations[routeId]) {
                if (routeId in this.savedRoutes) {
                  delete this.savedRoutes[routeId];
                  target.textContent = 'Save Route';
                  target.className = 'save-route-btn';
                } else {
                  // Store as string representation
                  const latLng = this.routeLocations[routeId];
                  this.savedRoutes[routeId] = `LatLng(${latLng.lat}, ${latLng.lng})`;
                  target.textContent = 'Unsave Route';
                  target.className = 'unsave-route-btn';
                }
                localStorage.setItem('savedRoutes', JSON.stringify(this.savedRoutes));
              }
            });
          }
        }, 0);
      }
    });
  }

  

  setActiveTab(tab: 'location' | 'route') {
    this.activeTab = tab;

    // Remove existing geocoder if it exists
    if (this.geocoder) {
      const geocoderContainer = document.querySelector('.leaflet-control-geocoder');
      if (geocoderContainer) {
        geocoderContainer.remove();
      }
      this.geocoder = null;
    }

    // Show location search if that tab is selected
    if (tab === 'location') {
      setTimeout(() => {
        this.showLocationSearch();
      }, 0);
    }
  }


  showLocationSearch() {
    if (!this.geocoder && this.map) {
      const geocoderControl = new Geocoder({
        collapsed: false,
        geocoder: new geocoders.Nominatim(),
      });


      const currentQuery = geocoderControl.options.query;

      console.log(currentQuery);


      // Add the geocoder to the map first
      geocoderControl.addTo(this.map);

      // Get the geocoder container after it's added to the map
      const geocoderContainer = document.querySelector('.leaflet-control-geocoder');
      if (geocoderContainer instanceof HTMLElement) {
        // Remove it from its original position
        geocoderContainer.remove();

        // Add it to our custom container
        const customContainer = document.getElementById('location-search-container');
        if (customContainer) {
          // Clear any existing content
          customContainer.innerHTML = '';

          // Append the geocoder
          customContainer.appendChild(geocoderContainer);

          // Apply styles
          geocoderContainer.style.width = '100%';
          geocoderContainer.style.margin = '0';
          geocoderContainer.style.border = 'none';
          geocoderContainer.style.boxShadow = 'none';
          geocoderContainer.style.display = 'block';


          // Style the input
          const searchInput = geocoderContainer.querySelector('input');
          if (searchInput instanceof HTMLElement) {
            searchInput.style.width = '100%';
            searchInput.style.padding = '8px';
            searchInput.style.borderRadius = '4px';
            searchInput.style.border = '1px solid #ddd';
          }
        }
      }

      this.geocoder = geocoderControl;
    }
  }


  onRouteSearch(): void {
    if (!this.routeSearchQuery) return;

    const routeId = this.routeSearchQuery.trim();
    const savedRoutesStr = localStorage.getItem('savedRoutes');

    if (!savedRoutesStr) {
      console.log('No saved routes found');
      return;
    }

    const savedRoutes = JSON.parse(savedRoutesStr);

    if (routeId in savedRoutes) {
      // Parse the LatLng string
      const latLngStr = savedRoutes[routeId];
      const matches = latLngStr.match(/LatLng\(([-\d.]+),\s*([-\d.]+)\)/);

      if (matches) {
        const lat = parseFloat(matches[1]);
        const lng = parseFloat(matches[2]);
        const latLng = new L.LatLng(lat, lng);

        this.map.setView(latLng, 12, {
          animate: true,
          duration: 1
        });

        const popupContent = `
          <div class="route-info">
            <div>Route ID: ${routeId}</div>
            <div>Location: ${latLngStr}</div>
            <div class="saved-route-badge">Saved Route</div>
          </div>
        `;

        const marker = L.marker(latLng)
          .addTo(this.map)
          .bindPopup(popupContent)
          .openPopup();

        setTimeout(() => {
          this.map.removeLayer(marker);
        }, 3000);
      }
    } else {
      console.log('Route not found in saved routes. Try saving a route first.');
    }
  }
  

  ngAfterViewInit(): void {
   // console.log(this.countriesData);
    this.initMap();
    setTimeout(() => {
      if (this.map) {
        this.map.invalidateSize(); //leaflet needs to know the size of the element on initialization, need to use invalidateSize()
  
      }
    }, 0);
  }


  ngOnInit() {
    //during init, load route from local storage, will replace later.
    const savedRoutesStr = localStorage.getItem('savedRoutes');
    if (savedRoutesStr) {
      this.savedRoutes = JSON.parse(savedRoutesStr);

      // Restore routeLocations from saved routes
      Object.entries(this.savedRoutes).forEach(([routeId, latLngStr]) => {
        const matches = latLngStr.match(/LatLng\(([-\d.]+),\s*([-\d.]+)\)/);
        if (matches) {
          const lat = parseFloat(matches[1]);
          const lng = parseFloat(matches[2]);
          this.routeLocations[routeId] = new L.LatLng(lat, lng);
        }
      });
    }
  }

}
