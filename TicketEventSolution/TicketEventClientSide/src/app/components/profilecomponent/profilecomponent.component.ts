import { Component , OnInit} from '@angular/core';
import { CustomerserviceService } from '../../service/customerservice.service'

@Component({
  selector: 'app-profilecomponent',
  templateUrl: './profilecomponent.component.html',
  styleUrl: './profilecomponent.component.css'
})


export class ProfilecomponentComponent implements OnInit{
  constructor(private customerService: CustomerserviceService) { }
  isEditMode: boolean = false;
  isEditToken: boolean = false;
  firstname: string = "UNDEFINED";
  lastname: string = "UNDEFINED";
  email: string = "UNDEFINED";
  password: string = "UNDEFINED";
  eventToken: string | null = "UNDEFINED";
  token: string | null = "UNDEFINED";
  ngOnInit() {

    //get email then get info
    this.token = this.customerService.getToken()
    console.log("profile token: ",this.token);
    this.customerService.getEmail().subscribe(
      response => {
        this.email = response.email; // Assuming response has an `email` field
      },
      error => {
        console.error("Error fetching email:", error);
      }
    );
    console.log("profile email: ", this.email);
    const updatedInfo = {};
    this.customerService.getCustomerInfoByEmail(this.email);
  }
  toggleEditMode() {
    this.isEditMode = !this.isEditMode;
    //console.log("Edit clicked: ", this.isEditMode);
  }
  toggleTokenEdit() {
    this.isEditToken = !this.isEditToken;
  }
  saveChanges() {
    const updatedInfo = {
      firstname: this.firstname,
      lastname: this.lastname,
      email: this.email,
      password: this.password,
      
    }
    //console.log("Edit clicked: ", this.isEditMode);
  }
  saveToken() {
    const updateToken = {
      eventToken: this.eventToken
    }
    
  }
}

