import { Component } from '@angular/core';
import * as L from 'leaflet';

@Component({
    selector: 'app-map',
    templateUrl: './map.component.html',
    styleUrl: './map.component.css',
    standalone: false
})
export class MapComponent {
  ngOnInit(): void {
    // Create the map instance
    const map = L.map('map').setView([51.505, -0.09], 13); // Default coordinates: [latitude, longitude], zoom level

    // Add OpenStreetMap tile layer
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 19
    }).addTo(map);

    // Optional: Add a marker to the map
    L.marker([51.5, -0.09]).addTo(map)
      .bindPopup('A pretty CSS3 popup.<br> Easily customizable.')
      .openPopup();
  }
}
