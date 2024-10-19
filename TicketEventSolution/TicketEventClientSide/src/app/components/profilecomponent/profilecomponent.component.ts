import { Component } from '@angular/core';
import { CustomerserviceService } from '../../service/customerservice.service'
@Component({
  selector: 'app-profilecomponent',
  templateUrl: './profilecomponent.component.html',
  styleUrl: './profilecomponent.component.css'
})


export class ProfilecomponentComponent {
  constructor(private customerService: CustomerserviceService) { }
  isEditMode: boolean = false;
  firstname: string = "UNDEFINED";
  lastname: string = "UNDEFINED";
  email: string = "UNDEFINED";
  password: string = "UNDEFINED";
  eventToken: string = "UNDEFINED";
  retrieveData() {
    
  }
  toggleEditMode() {
    this.isEditMode = !this.isEditMode;
    //console.log("Edit clicked: ", this.isEditMode);
  }
  saveChanges() {
    const updatedInfo = {
      firstname: this.firstname,
      lastname: this.lastname,
      email: this.email,
      password: this.password

    }
    //console.log("Edit clicked: ", this.isEditMode);
  }
}

