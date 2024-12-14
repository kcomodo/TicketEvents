import { Component } from '@angular/core';
import { CustomerserviceService } from '../../service/customerservice.service';
import { Route, Router } from "@angular/router";
@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrl: './register.component.css',
    standalone: false
})
export class RegisterComponent {
  firstname: string = "";
  lastname: string = "";
  email: string = "";
  password: string = "";
  confirmPassword: string = "";
  feed_token: string = "";
  constructor(private router: Router, private customerService: CustomerserviceService) { }
  doPasswordsMatch(): boolean {
    return this.password === this.confirmPassword;
  }
  isEmailValid(): boolean {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailRegex.test(this.email);
  }
  onRegister(): void {
    this.customerService.registerCustomer(this.firstname, this.lastname, this.email, this.password, this.feed_token).subscribe(
      (response) => {
        console.log('Registration successful:', response);
        this.router.navigate(['/home']);
        // Handle success, maybe navigate to another page or show a message
      },
      (error) => {
        console.error('Registration failed:', error);
        // Handle error, show error message
      }
    );
  }

}
