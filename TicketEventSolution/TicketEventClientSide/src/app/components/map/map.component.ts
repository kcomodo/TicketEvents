import { Component , AfterViewInit } from '@angular/core';
import * as L from 'leaflet';
import * as countries from 'countries-list';
import { TuiDay, TuiTime } from '@taiga-ui/cdk';
import { Geocoder, geocoders } from 'leaflet-control-geocoder';
import "leaflet/dist/leaflet.css";
import "leaflet-control-geocoder/dist/Control.Geocoder.css";
import "leaflet.vectorgrid";

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrl: './map.component.css',
  standalone: false
})
export class MapComponent implements AfterViewInit {
  private map!: L.Map; // Explicitly define the type as L.Map
  countriesData = countries.countries;  // Get country data
  //Leaflet guide shows how the code is generated.
  private polygonLayers: L.Polygon[] = [];
  private initMap(): void {

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
    // const mapBox = L.vectorGrid.protobuf("https://transit.land/api/v2/tiles/routes/tiles/{z}/{x}/{y}.pbf?apikey=Z2xK57toXiR4t1cLlMvfC4fofM4ZhmVV", {
    /*
    const mapBox = L.vectorGrid.protobuf(
      `https://transit.land/api/v2/tiles/routes/tiles/{z}/{x}/{y}.pbf?operator_onestop_id=UPV&apikey=Z2xK57toXiR4t1cLlMvfC4fofM4ZhmVV`,
      {
      vectorTileLayerStyles: {
        routes: {
          weight: 3,
          color: "#ff0000",
          opacity: 1,
          lineCap: 'round',   // Round line endings
          lineJoin: 'round'   // Round line joints
        }

      },
        maxZoom: 14,
        interactive: true,      // Make routes clickable
        getFeatureId: (f: any) => f.properties.route_id,
     

      }).addTo(this.map);
    // Optional: Add click handler for routes
    this.map.on('click', (e: any) => {
      if (e.layer && e.layer.properties) {
        const routeInfo = e.layer.properties;
        L.popup()
          .setLatLng(e.latlng)
          .setContent(`Route: ${routeInfo.route_name || 'Unknown'}`)
          .openOn(this.map);
      }
    });
    */
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
        ${props.route_type ? `Type: ${props.route_type}<br>` : ''}
      </div>
    `;

        L.popup()
          .setLatLng(e.latlng)
          .setContent(content)
          .openOn(this.map);
      }
    });
    /*
    const apiResponse = {
      "agencies": [
        {
          "agency_id": "UPV",
          "agency_name": "UNIBUSPV",
          "geometry": {
            "coordinates": [
              [
                [-105.314125, 20.51288],
                [-105.31455, 20.52034],
                [-105.24801, 20.700443],
                [-105.24793, 20.700596],
                [-105.1504, 20.79553],
                [-105.107445, 20.82609],
                [-105.0981, 20.823923],
                [-105.09651, 20.82331],
                [-105.096375, 20.82315],
                [-105.21984, 20.594149],
                [-105.257225, 20.558283],
                [-105.28925, 20.530539],
                [-105.314125, 20.51288]
              ]
            ],
       
          }
        }
      ]
    };

    apiResponse.agencies.forEach(agency => {
      // Convert the coordinates to LatLngTuple[]
      const latLngCoords: L.LatLngTuple[] = agency.geometry.coordinates[0].map(coord =>
        [coord[1], coord[0]] as L.LatLngTuple  // Swap lat/lng because GeoJSON uses [lng, lat]
      );

      const polygon = L.polygon(latLngCoords, {
        color: 'blue',
        weight: 3,
        opacity: 0.7
      }).addTo(this.map);

      polygon.bindPopup(agency.agency_name);
      this.polygonLayers.push(polygon);
    });
 */

    new Geocoder({
      geocoder: new geocoders.Nominatim(),
      position: 'topleft',
    }).addTo(this.map);

  }

  constructor() { }

  ngAfterViewInit(): void {
    console.log(this.countriesData);
    this.initMap();
    setTimeout(() => {
      if (this.map) {
        this.map.invalidateSize(); //leaflet needs to know the size of the element on initialization, need to use invalidateSize()
  
      }
    }, 0);
  }


}
