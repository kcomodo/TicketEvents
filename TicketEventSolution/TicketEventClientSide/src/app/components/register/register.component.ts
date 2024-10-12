import { Component } from '@angular/core';
import { CustomerserviceService } from '../../service/customerservice.service';
import { Route, Router } from "@angular/router";
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  firstname: string = "";
  lastname: string = "";
  email: string = "";
  password: string = "";
  constructor(private router: Router, private customerService : CustomerserviceService) {}
  onSubmit(): void{
    this.customerService.registerCustomer(this.firstname, this.lastname, this.email, this.password);
  }
  
}
