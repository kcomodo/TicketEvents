import { Component , OnInit} from '@angular/core';
import { CustomerserviceService } from '../../service/customerservice.service';
@Component({
  selector: 'app-profilecomponent',
  templateUrl: './profilecomponent.component.html',
  styleUrl: './profilecomponent.component.css'
})


export class ProfilecomponentComponent implements OnInit {
  constructor(private customerService: CustomerserviceService) { }
  isEditMode: boolean = false;
  isEditToken: boolean = false;
  firstname: string | null= "";
  lastname: string | null= "";
  email: string = "";
  password: string = "";
  customerId: string = "";
  eventToken: string | null = "UNDEFINED";
  token: string | null = "UNDEFINED";
  ngOnInit() {

    //get email then get info
    this.grabInfo();
    this.email = this.customerService.getEmailSaved();
    console.log("email saved:", this.email);
    this.displayInfo();
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
  grabInfo() {
    this.token = this.customerService.getToken()
    console.log("profile token: ", this.token);
    this.customerService.getEmail().subscribe(
      response => {
        this.email = response.email; // Assuming response has an `email` field
        this.customerService.setEmail(this.email);
      },
      error => {
        console.error("Error fetching email:", error);
      }

    );

  }
  displayInfo() {
    // console.log("Console log:" ,this.customerService.getCustomerInfoByEmail(this.email));
   /*
    this.customerService.getCustomerInfoByEmail(this.email).subscribe((response) => {
      console.log(response.firstname, response.lastname, response.password, response.email);
      this.firstname = response.firstname;
      this.lastname = response.lastname;
      this.password = response.password;
    });
    */
  
    this.customerService.getCustomerInfoByEmail(this.email).subscribe(
      (data) => {
        console.log('Customer data received in component:', data);
        this.firstname = data.firstName;
        this.lastname = data.firstName;
        this.password = data.password;
        console.log("checkpoint", this.customerId ,this.firstname, this.lastname, this.password);
      },
      (error) => {
        console.error('Error fetching customer data:', error);
      }
    );
  }
}

