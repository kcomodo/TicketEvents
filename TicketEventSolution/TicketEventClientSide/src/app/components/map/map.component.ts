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
  locationSearchQuery: string = '';
  activeTab: 'location' | 'route' = 'location';
  routeSearchQuery: string = '';
  private geocoder: Geocoder | null = null;
  coordinates: number[][] | null = null;
  private tooltip: L.Popup | null = null;
  private map!: L.Map; // Explicitly define the type as L.Map
  countriesData = countries.countries;  // Get country data
  //Leaflet guide shows how the code is generated.
  private polygonLayers: L.Polygon[] = [];
  private initMap(): void {
    this.customerService.getAgencies().subscribe((response) => {
      if (response && response.agencies) {
        // Access the first agency's geometry.coordinates
        const agency = response.agencies[0];
        this.coordinates = agency.geometry.coordinates[0]; // Grabbing the first array of coordinates
        console.log(this.coordinates); //Testing to see if coordinates work
      }
    });
    //https://leafletjs.com/examples/quick-start/
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
    //needed to install @types/leaflet.vectorgrid
    //The plugin only supported js so we need @types
    //replace the api key later when fully finished
    //https://leaflet.github.io/Leaflet.VectorGrid/vectorgrid-api-docs.html#vectorgrid-protobuf
    const mapBox = L.vectorGrid.protobuf(
      //operator_onestop_id=UPV&apikey=Z2xK57toXiR4t1cLlMvfC4fofM4ZhmVV
      `https://transit.land/api/v2/tiles/routes/tiles/{z}/{x}/{y}.pbf?&apikey=Z2xK57toXiR4t1cLlMvfC4fofM4ZhmVV`,
     // https://transit.land/api/v2/rest/agencies?adm0_name=Mexico&apikey=Z2xK57toXiR4t1cLlMvfC4fofM4ZhmVV
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
        getFeatureId: (f: any) => f.properties.route_id,
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

        // Create tooltip content
        const content = `
          <div class="route-info">
            ${props.route_short_name ? `<div>Route: ${props.route_short_name}</div>` : ''}
            ${props.route_long_name ? `<div>${props.route_long_name}</div>` : ''}
            ${props.route_id ? `<div>ID: ${props.route_id}</div>` : ''}
          </div>
        `;

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
      console.log('Route clicked:', e); // For debugging
      if (e.layer && e.layer.properties) {
        const props = e.layer.properties;

        // Create tooltip content
        const content = `
          <div class="route-info">
            ${props.route_short_name ? `<div>Route: ${props.route_short_name}</div>` : ''}
            ${props.route_long_name ? `<div>${props.route_long_name}</div>` : ''}
            ${props.route_id ? `<div>ID: ${props.route_id}</div>` : ''}
          </div>
    `;

        L.popup()
          .setLatLng(e.latlng)
          .setContent(content)
          .openOn(this.map);
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
        query: 'Mexico',
        geocoder: new geocoders.Nominatim(),
      });


      const currentQuery = geocoderControl.options.query;
 
      console.log(currentQuery);

      this.onRouteSearch();
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
  
  hideLocationSearch() {
    /*
    const geocoderContainer = document.querySelector('.leaflet-control-geocoder');
    if (geocoderContainer instanceof HTMLElement) {
      geocoderContainer.style.display = 'none';
    }
    */
  }
  

  

  onRouteSearch() {
    //testing
    /*
    console.log('Searching for location:', this.locationSearchQuery);
    console.log('Searching for route:', this.routeSearchQuery);
    */
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


}
