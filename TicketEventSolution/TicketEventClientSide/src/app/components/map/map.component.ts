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

  private routeLocations: { [key: string]: L.LatLng } = {}; //Stored as route coordinates
  private routeLocations2: { [key: string]: L.LatLng } = {}; //Stored as route coordinates
  locationSearchQuery: string = '';
  activeTab: 'location' | 'route' = 'location';
  routeSearchQuery: string = '';
  private geocoder: Geocoder | null = null;
  private tooltip: L.Popup | null = null;
  private map!: L.Map; // Explicitly define the type as L.Map
  private email: string = '';
  private feed_token: string = '';
  routeId: string = '';
  customer_id: string = '';
  routeIds: number[] = [];  // or Array<number>
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

    this.email = this.customerService.getEmailSaved();
    // console.log(this.email);
    this.customerService.getFeedToken(this.email).subscribe(
      (response) => {
        this.feed_token = response.feed_token;
 

    const mapBox = L.vectorGrid.protobuf(
      `https://transit.land/api/v2/tiles/routes/tiles/{z}/{x}/{y}.pbf?include_geometry=true&apikey=${this.feed_token}`,
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
          this.getInfo(() => {
        //    console.log("Testing:", this.routeLocations); // Now it's available
          });
       //   console.log("Testing:", this.routeLocations);
      if (e.layer && e.layer.properties) {
        const props = e.layer.properties;
    //    console.log("Info: ", props,e.latlng);
        this.routeLocations2[props.route_id] = e.latlng;
      //  console.log("Temp storage", this.routeLocations2);
        console.log("Temp storage2", props.route_id, this.routeLocations2[props.route_id].lat, this.routeLocations2[props.route_id].lng);
        //console.log(this.routeLocations[props.route_id]);
       // console.log(props.route_id);
        const saveButtonHtml = props.route_id in this.routeLocations
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
              const tempId = target.getAttribute('data-route-id');

              if (tempId !== null && tempId in this.routeLocations) {
                //  console.log("routeId in routeLocation", routeId, "+", this.routeLocations)
                //  delete this.savedRoutes[this.routeId];
                //   console.log("routeRemoved routeId: ", this.routeId);
                console.log("Testing Route exists, deleting:", this.routeLocations);
                  target.textContent = 'Save Route';
                  target.className = 'save-route-btn';
                this.customerService.deleteRouteInfo(this.customer_id, tempId);
                } else {
                  // Store as string representation
                 // console.log("routeId not in routeLocation, Saving");
                 // console.log("Temp storage3", props.route_id, this.routeLocations2[props.route_id].lat, this.routeLocations2[props.route_id].lng);
              //    const latLng = this.routeLocations[this.routeId];
   
               //   this.savedRoutes[this.routeId] = `LatLng(${latLng.lat}, ${latLng.lng})`;
                  this.customerService.getCustomerInfoByEmail(this.email).subscribe({
                    next: (data) => {
                      this.customer_id = data.customer_id;
                     // console.log("Customer data received:", data);
                      //  console.log("Customer id:", this.customer_id);
                      const tempLat = this.routeLocations2[props.route_id].lat;
                      const tempLng = this.routeLocations2[props.route_id].lng;
                      const routes_id = props.route_id;
                      console.log(tempLat, tempLng);
                      console.log(routes_id);
                      this.customerService.saveRouteInfo(this.customer_id, routes_id, tempLat, tempLng);
                      target.textContent = 'Unsave Route';
                      target.className = 'unsave-route-btn';
                    },
                    error: (error) => {
                      console.error("Error fetching customer info:", error);
                      // Handle the error (e.g., display an error message to the user)
                    }
                  });
             
                }
         
              
            });
          }
        }, 0);
      }
    });
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
    this.email = this.customerService.getEmailSaved();
    this.customerService.getCustomerInfoByEmail(this.email).subscribe({
      next: (data) => {
        this.customer_id = data.customer_id;

        this.customerService.getRouteInfo(this.customer_id).subscribe({
          next: (routeData) => {
            this.routeIds = routeData.map((route: any) => route.routes_Id);

            // Store routes in routeLocations object
            routeData.forEach((route: any) => {
              const routeId = route.routes_Id.toString();
            this.routeLocations[routeId] = new L.LatLng(route.latitude, route.longitude);
            });

            // After loading routes, perform the search
            if (this.routeSearchQuery) {
              const routeId = this.routeSearchQuery.trim();

              if (routeId in this.routeLocations) {
                const latLng = this.routeLocations[routeId];

                // Set map view to the found location
                this.map.setView(latLng, 12, {
                  animate: true,
                  duration: 1
                });

                // Create popup content
                const popupContent = `
                <div class="route-info">
                  <div>Route ID: ${routeId}</div>
                  <div>Location: ${latLng.toString()}</div>
                  <div class="saved-route-badge">Saved Route</div>
                </div>
              `;

                // Add and remove marker with timeout
                const marker = L.marker(latLng)
                  .addTo(this.map)
                  .bindPopup(popupContent)
                  .openPopup();

                setTimeout(() => {
                  this.map.removeLayer(marker);
                }, 3000);
              } else {
                console.log('Route not found. Please check the route ID and try again.');
              }
            }
          },
          error: (error) => {
            console.error("Error fetching route info:", error);
          }
        });
      },
      error: (error) => {
        console.error("Error fetching customer info:", error);
      }
    });
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
    this.email = this.customerService.getEmailSaved();
    this.getInfo();
  }

  getInfo(callback?: () => void) {
    this.customerService.getCustomerInfoByEmail(this.email).subscribe({
      next: (data) => {
        this.customer_id = data.customer_id;

        // Now subscribe to getRouteInfo
        this.customerService.getRouteInfo(this.customer_id).subscribe({
          next: (routeData) => {
            this.routeIds = routeData.map((route: any) => route.routes_Id);
            this.routeLocations = {}; // Reset before populating

            routeData.forEach((route: any) => {
              const routeId = route.routes_Id.toString();
              this.routeLocations[routeId] = new L.LatLng(route.latitude, route.longitude);
            });

            console.log("Route Locations Updated:", this.routeLocations);
            //  console.log("Route Locations Updated:", this.routeLocations[75]);
          /*
            if ('75' in this.routeLocations) {
              console.log("Route 75 exists:", this.routeLocations['75']);
            } else {
              console.log("Route 75 does not exist");
            }
*/
            if (callback) callback();
          },
          error: (error) => {
            console.error("Error fetching route info:", error);
          }
        });
      },
      error: (error) => {
        console.error("Error fetching customer info:", error);
      }
    });
  }

}


