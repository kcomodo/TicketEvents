import { Component , AfterViewInit } from '@angular/core';
import * as L from 'leaflet';

@Component({
    selector: 'app-map',
    templateUrl: './map.component.html',
    styleUrl: './map.component.css',
    standalone: false
})
export class MapComponent implements AfterViewInit {
  private map!: L.Map; // Explicitly define the type as L.Map

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
  }

  constructor() { }

  ngAfterViewInit(): void {
    this.initMap();
    setTimeout(() => {
      if (this.map) {
        this.map.invalidateSize(); //leaflet needs to know the size of the element on initialization, need to use invalidateSize()
  
      }
    }, 0);
  }
  onResize(): void { //This is for dynamic resizing if the browser have been modified.
    if (this.map) {
      this.map.invalidateSize();
    }
  }

}
