import { Component, inject } from '@angular/core';
import { CustomerserviceService } from '../../service/customerservice.service';
import { Route, Router } from "@angular/router";
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatStepperModule } from '@angular/material/stepper';
import { MatButtonModule } from '@angular/material/button';
import { FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrl: './register.component.css',
    standalone: false
})
export class RegisterComponent {

  feed_token: string = "";
  constructor(private router: Router, private customerService: CustomerserviceService) { }
  /*
  onSubmit() {
    const firstname = this.firstFormGroup.get("firstCtrl")?.value;
    const lastname = this.firstFormGroup.get("lastCtrl")?.value;
    const email = this.secondFormGroup.get("emailCtrl")?.value;
    const password = this.thirdFormGroup.get("passwordCtrl")?.value;
    console.log("First Name: ", firstname);
    console.log("Last Name: ", lastname);
    console.log("Email: ", email);
    console.log("Password: ", password);
  }
  */
  isPasswordValid(): boolean {
    const password = this.thirdFormGroup.get("passwordCtrl")?.value;
    const password2 = this.thirdFormGroup.get("passwordCtrlCheck")?.value;
    // Password requirements: at least 8 characters, one uppercase, one number
    const passwordRegex = /^(?=.*[A-Z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$/;

    // const passwordRegex = /^.{8}$/;
    if (password == password2) {
      return typeof password === 'string' && passwordRegex.test(password);
    }
    return false;
  }
  isEmailValid(): boolean {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    const email = this.secondFormGroup.get("emailCtrl")?.value;
    const email2 = this.secondFormGroup.get("emailCtrlCheck")?.value;
    if (email == email2) {
      return typeof email === 'string' && emailRegex.test(email);
    }
    return false;

  }
  onRegister(): void {
    const firstname = this.firstFormGroup.get("firstCtrl")?.value;
    const lastname = this.firstFormGroup.get("lastCtrl")?.value;
    const email = this.secondFormGroup.get("emailCtrl")?.value;
    const password = this.thirdFormGroup.get("passwordCtrl")?.value;
    this.customerService.registerCustomer(
      this.firstFormGroup.get("firstCtrl")?.value || "",
      this.firstFormGroup.get("lastCtrl")?.value || "",
      this.secondFormGroup.get("emailCtrl")?.value || "",
      this.thirdFormGroup.get("passwordCtrl")?.value || "",
      this.feed_token

    ).subscribe(
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

  private _formBuilder = inject(FormBuilder);

  firstFormGroup = this._formBuilder.group({
    firstCtrl: ['', Validators.required],
    lastCtrl: ['', Validators.required],
  });
  secondFormGroup = this._formBuilder.group({
    emailCtrl: ['', Validators.required],
    emailCtrlCheck: ['', Validators.required],
  });
  thirdFormGroup = this._formBuilder.group({
    passwordCtrl: ['', Validators.required],
    passwordCtrlCheck: ['', Validators.required],
  });
  isLinear = false;

}
