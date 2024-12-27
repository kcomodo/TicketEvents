import { Component , AfterViewInit } from '@angular/core';
import * as L from 'leaflet';
import * as countries from 'countries-list';
import { TuiDay, TuiTime } from '@taiga-ui/cdk';
import { Geocoder, geocoders } from 'leaflet-control-geocoder';
import "leaflet/dist/leaflet.css";
import "leaflet-control-geocoder/dist/Control.Geocoder.css";



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

  private initMap(): void {
    this.map = L.map('map', {
      center: [40.8282, -98.5795],
      zoom: 5
    });

    const tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 18,
      minZoom: 1,
      attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    });

    tiles.addTo(this.map);

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
