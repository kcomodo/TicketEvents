import { Component, OnInit } from '@angular/core';
import { CustomerserviceService } from '../app/service/customerservice.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'Ticket Tracker';
  constructor(private customerService: CustomerserviceService, private router: Router) { }
  ngOnInit() {
    this.checkLogin();
  }
  checkLogin() {
    if (this.customerService.getToken() != null) {
      return true;
      console.log("True");
    }
    else {
      return false;
      console.log("False");
    }
  }
  checkLogout() {
    this.customerService.logout();

  }
}

