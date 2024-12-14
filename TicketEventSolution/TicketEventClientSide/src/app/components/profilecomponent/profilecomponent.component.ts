import { Component, OnInit } from '@angular/core';
import { CustomerserviceService } from '../../service/customerservice.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-profilecomponent',
    templateUrl: './profilecomponent.component.html',
    styleUrls: ['./profilecomponent.component.css'],
    standalone: false
})

export class ProfilecomponentComponent implements OnInit {
  isEditMode: boolean = false;
  isEditToken: boolean = false;
  customer_firstname: string | null = "";
  customer_lastname: string | null = "";
  customer_email: string = "";
  customer_password: string = "";
  feed_token: string | null = "UNDEFINED";
  token: string | null = "UNDEFINED";
  targetemail: string = "";
  checkValidation: boolean = false;
  constructor(private customerService: CustomerserviceService, private router: Router) { }

  ngOnInit() {
    // Get email then retrieve the customer info
    this.customer_email = this.customerService.getEmailSaved();
    if (this.customer_email) {
      this.targetemail = this.customer_email; // Set the targetemail only once
      console.log("original email", this.targetemail);
      this.displayInfo();  // Call displayInfo if email is available
    } else {
      this.router.navigate(['/login']); // Navigate to login if email is not available
    }
  }

  // Toggle edit mode for customer info
  toggleEditMode() {
    this.isEditMode = !this.isEditMode;
  }

  // Toggle edit mode for feed token
  toggleTokenEdit() {
    this.isEditToken = !this.isEditToken;
  }


  saveChanges() {
    console.log('Current feed_token:', this.feed_token); // Debugging step

    // If feed_token is null, show an error
    if (!this.feed_token) {
      alert('Feed token cannot be null or empty.');
      return;
    }

    const updatedInfo = {
      customer_firstname: this.customer_firstname,
      customer_lastname: this.customer_lastname,
      customer_email: this.customer_email,
      customer_password: this.customer_password,
      feed_token: this.feed_token
    };

    console.log('Info changed:', updatedInfo);
    console.log('Original email:', this.targetemail);

    // Validate the feed token first
    
    this.customerService.validateFeedToken(this.feed_token).subscribe({
      next: (response) => {
        console.log('Token Valid:', response);
        this.customerService.updateCustomerInfo(updatedInfo, this.targetemail).subscribe({
          next: (response) => {
            console.log('Update successful:', response);
            this.isEditMode = false;  // Exit edit mode
            this.displayInfo();       // Refresh the displayed information
          },
          error: (error) => {
            console.error('Profile component update failed:', error);
            // Optionally display a user-friendly error message here
          }
        });
      },
      error: (error) => {
        console.log('Token Invalid:', error);
        alert('The provided feed token is invalid. Please enter a valid token.');
      
      }
    });
    
    
  



    
    
  }


  displayInfo() {
    this.customerService.getCustomerInfoByEmail(this.customer_email).subscribe({
      next: (data) => {
        this.customer_firstname = data.customer_firstname;
        this.customer_lastname = data.customer_lastname;
        this.customer_password = data.customer_password;
        this.feed_token = data.feed_token;
        console.log("Customer data received:", data);
      },
      error: (error) => {
        console.error("Error fetching customer info:", error);
        // Handle the error (e.g., display an error message to the user)
      }
    });
  }
}
