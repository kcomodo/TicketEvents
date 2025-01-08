import { Component , AfterViewInit } from '@angular/core';
import * as L from 'leaflet';
import * as countries from 'countries-list';
import { TuiDay, TuiTime } from '@taiga-ui/cdk';
import { Geocoder, geocoders } from 'leaflet-control-geocoder';
import "leaflet/dist/leaflet.css";
import "leaflet-control-geocoder/dist/Control.Geocoder.css";
import "leaflet.vectorgrid";
import { CustomerserviceService } from '../../service/customerservice.service';
@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrl: './map.component.css',
  standalone: false
})
export class MapComponent implements AfterViewInit {
  constructor(private customerService: CustomerserviceService) {
  }
  coordinates: number[][] | null = null;
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


    const mapBox = L.vectorGrid.protobuf(
      `https://transit.land/api/v2/tiles/routes/tiles/{z}/{x}/{y}.pbf?operator_onestop_id=UPV&apikey=Z2xK57toXiR4t1cLlMvfC4fofM4ZhmVV`,
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

    mapBox.on('mouseover', (e: any) => {
      if (e.layer) {
        e.layer.setStyle({
          weight: 5,
          color: '#FFD700'
        });
      }
    });
    mapBox.on('mouseout', (e: any) => {
      if (e.layer) {
        e.layer.setStyle({
          weight: 3,
          color: '#ff0000'
        });
      }
    });
    // Attach click handler to the vector tile layer, not the map
    mapBox.on('click', (e: any) => {
      console.log('Route clicked:', e); // For debugging
      if (e.layer && e.layer.properties) {
        const props = e.layer.properties;
        const content = `
      <div>
        <strong>Route: ${props.route_name || 'Unknown'}</strong><br>
        ${props.route_long_name ? `Long name: ${props.route_long_name}<br>` : ''}
        ${props.route_short_name ? `Short name: ${props.route_short_name}<br>` : ''}
      </div>
    `;

        L.popup()
          .setLatLng(e.latlng)
          .setContent(content)
          .openOn(this.map);
      }
    });
 

    new Geocoder({
      geocoder: new geocoders.Nominatim(),
      position: 'topleft',
    }).addTo(this.map);

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
