import { Component } from '@angular/core';
import { CustomerserviceService } from "../../service/customerservice.service";
import { accessGuard } from "../../guards/access.guard";
import { Router } from "@angular/router";
@Component({
  selector: 'app-logincomponent',
  templateUrl: './logincomponent.component.html',
  styleUrl: './logincomponent.component.css'
})
export class LogincomponentComponent {
  email: string = "";
  password: string = "";
  constructor(private customerService: CustomerserviceService, private router: Router) {

  }
}
