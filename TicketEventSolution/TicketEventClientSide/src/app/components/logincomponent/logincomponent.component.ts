import { Component, OnInit } from '@angular/core';
import { CustomerserviceService } from "../../service/customerservice.service";
import { accessGuard } from "../../guards/access.guard";
import { Router } from "@angular/router";
import { FormBuilder, Validators } from '@angular/forms';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
@Component({
  selector: 'app-logincomponent',
  templateUrl: './logincomponent.component.html',
  styleUrl: './logincomponent.component.css'
})
export class LogincomponentComponent {
  email: string = "";
  password: string = "";
  errorMessage: string = '';
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  passwordFormControl = new FormControl('', [Validators.required]);
  constructor(private router: Router, private customerService: CustomerserviceService) { }
  onLogin(): void {
   // console.log('Login clicked with email: ', this.email, ' and password: ', this.password);
    this.customerService.validateLogin(this.email, this.password).subscribe(
      (response) => {
        // console.log(response);
        // console.log(this.email, this.password);
        // Handle successful login, e.g., redirect to dashboard
        if (response == true) {
          /*
          this.customerService.cu(this.email).subscribe(

            userId => {
              //  console.log("userid", userId);

            });

         
          */
          this.customerService.setEmail(this.email);
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
