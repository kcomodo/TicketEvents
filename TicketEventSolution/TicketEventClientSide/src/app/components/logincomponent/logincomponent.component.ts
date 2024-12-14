import { Component, OnInit } from '@angular/core';
import { CustomerserviceService } from "../../service/customerservice.service";
import { accessGuard } from "../../guards/access.guard";
import { Router } from "@angular/router";
import { FormBuilder, Validators } from '@angular/forms';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import * as countries from 'countries-list';


@Component({
    selector: 'app-logincomponent',
    templateUrl: './logincomponent.component.html',
    styleUrl: './logincomponent.component.css',
    standalone: false
})
export class LogincomponentComponent {
  customer_email: string = "";
  customer_password: string = "";
  errorMessage: string = '';
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  passwordFormControl = new FormControl('', [Validators.required]);
  countriesData = countries.countries;  // Get country data

  constructor(private router: Router, private customerService: CustomerserviceService) {
  }
  onLogin(): void {
    console.log('Login clicked with email: ', this.customer_email, ' and password: ', this.customer_password);

    console.log(this.countriesData);  // Logs country names and codes



    this.customerService.validateLogin(this.customer_email, this.customer_password).subscribe(
      (response) => {
         console.log(response);
         console.log(this.customer_email, this.customer_password);
        // Handle successful login, e.g., redirect to dashboard
        if (response == true) {
          /*
          this.customerService.cu(this.email).subscribe(

            userId => {
              //  console.log("userid", userId);

            });

         
          */
          this.customerService.setEmail(this.customer_email);
          // console.log("Login successful, Email saved:", this.emailService.getEmail());
          this.router.navigate(['/profile']);
        }

      },
      (error) => {
        //console.error('Login failed', error);
        // Handle login error, e.g., display error message
        this.errorMessage = 'Invalid Username or Password';


      }
    );
  }


  
}
