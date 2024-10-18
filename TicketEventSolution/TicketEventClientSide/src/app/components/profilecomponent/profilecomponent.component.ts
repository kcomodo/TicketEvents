import { Component } from '@angular/core';

@Component({
  selector: 'app-profilecomponent',
  templateUrl: './profilecomponent.component.html',
  styleUrl: './profilecomponent.component.css'
})
export class ProfilecomponentComponent {
  isEditMode: boolean = false;
  firstname: string = "UNDEFINED";
  lastname: string = "UNDEFINED";
  email: string = "UNDEFINED";
  password: string = "UNDEFINED";
  eventToken: string = "UNDEFINED";
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

