import { Component , OnInit} from '@angular/core';
import { CustomerserviceService } from '../../service/customerservice.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-profilecomponent',
  templateUrl: './profilecomponent.component.html',
  styleUrl: './profilecomponent.component.css'
})


export class ProfilecomponentComponent implements OnInit {
  constructor(private customerService: CustomerserviceService, private router : Router) { }
  isEditMode: boolean = false;
  isEditToken: boolean = false;
  firstname: string | null= "";
  lastname: string | null= "";
  email: string = "";
  password: string = "";
  customerId: string = "";
  eventToken: string | null = "UNDEFINED";
  token: string | null = "UNDEFINED";
  targetemail: string = "";
  ngOnInit() {
    //get email then get info
    this.email = this.customerService.getEmailSaved();
    if (this.email) {

      this.displayInfo();  // Only call if email is available
    } else {
      // Optionally handle the case where email is missing
      this.router.navigate(['/login']);
    }
    this.targetemail = this.email;
  //  console.log("email saved:", this.email);
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
      FirstName: this.firstname,
      LastName: this.lastname,
      Email: this.email,
      Password: this.password,
    };

    this.customerService.updateCustomerInfo(updatedInfo, this.targetemail).subscribe({
      next: (response) => {
        console.log('Update successful:', response);
        this.isEditMode = false;  // Exit edit mode
        this.targetemail = this.email;
        this.displayInfo();       // Refresh the displayed information
      },
      error: (error) => {
        console.error('Update failed:', error);
        // Handle error (e.g., show error message to user)
      }

    });
  }
  
  saveToken() {
    const updateToken = {
      eventToken: this.eventToken
    };
    this.customerService.updateTokenFeed(updateToken, this.targetemail).subscribe({
      next: (response) => {
        console.log('Token update successful:', response);
        this.isEditMode = false;  // Exit edit mode
        this.displayInfo();       // Refresh the displayed information
      },
      error: (error) => {
        console.error('Token update failed:', error);
        // Handle error (e.g., show error message to user)
      }
    });
  }
  grabInfo() {
    this.token = this.customerService.getToken()
   // console.log("profile token: ", this.token);
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
    this.customerService.getCustomerInfoByEmail(this.email).subscribe(
      (data) => {
       // console.log('Customer data received in component:', data);
        this.firstname = data.firstName;
        this.lastname = data.lastName;
        this.password = data.password;
        this.eventToken = data.tokenFeed;
        console.log("checkpoint",this.firstname, this.lastname, this.password);
      }
    );
  }
}

