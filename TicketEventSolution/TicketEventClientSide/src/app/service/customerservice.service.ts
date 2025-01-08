import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service'; //npm install ngx-cookie-service --save --force used for read, saet, and delete browser cookies
import { catchError, first, tap } from 'rxjs/operators';
import { Token } from '@angular/compiler';
import { map } from 'rxjs/operators';
import { response } from 'express';
import { HttpHeaders } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { Observable, empty, throwError } from 'rxjs';
import { Component, OnInit } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class CustomerserviceService implements OnInit {

  //default url for the api which is swagger
  private token: string | null = null;
  private isAuthenticated = false;
  private baseUrl = "https://localhost:7240/api/Values";
  private tokenSaved = 'tokenSaved';
  private emailSaved = 'emailSaved';
  private email: string | null = null;
  constructor(private http: HttpClient, private cookieService: CookieService) {
  }
  
  ngOnInit() {
    console.log("Authentication: ", this.isAuthenticated);
  }

  validateLogin(customer_email: string, customer_password: string): Observable<boolean>
  {
    //console.log("customer service now: ", email, password);
    //keep it as a post because the information is not visible to the URl, and is more secured
    //old method
    //return this.http.post<{ token: string }>(`${this.baseUrl}/ValidateLogin?email=${email}&password=${password}`, "")
    //new method, this is more secured instead of directly accessing the url, we are sending it into the body of the request
    return this.http.post<{ token: string }>(`${this.baseUrl}/ValidateLogin`, {customer_email, customer_password}, {
      headers: { 'Content-Type': 'application/json' }
    })
      .pipe(
        tap(response => {
          //console.log(response);
        if (response.token) {
          this.token = response.token;
          this.isAuthenticated = true; // Set authentication status based on response
          //  console.log("isLoggedin received: ", this.isAuthenticated);
         //  console.log("Token received and saved: ", this.token);
          this.cookieService.set(this.tokenSaved, this.token, { path: '/' });
        }
        else if (this.getToken() != null) {
          this.isAuthenticated = true;
      //    console.log("token is not null");
      //    console.log("token not null and: ", email,password)
        }
        else {
          this.isAuthenticated = false;
          //  console.log(response.token);
          //  console.log("isLoggedin failed: ", this.isAuthenticated);
        }

      }),

      map(response => !!response.token) // Transform response to a boolean
    );

  
  }
  registerCustomer(customer_firstname: string, customer_lastname: string, customer_email: string, customer_password: string, feed_token : any) {
    console.log("registerCustomer called");

    return this.http.post<any>(`${this.baseUrl}/AddCustomer`, { customer_firstname, customer_lastname, customer_email, customer_password, feed_token }, {
      headers: { 'Content-Type': 'application/json' }
    })
  }


  getToken(): string | null {
    //grab the token from the cookie service and then return it to be stored
    //cookieService is a library that allows us to store the token
   return this.cookieService.get(this.tokenSaved) || null;
  }
  // Need to decode token to recieve email
  getEmail(): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.token}`
    });
    /*
    console.log("Service token: ", this.token);
    console.log("Header:", headers);
    */
    return this.http.get<string>(`${this.baseUrl}/GetCustomerEmail`, { headers });
  }
  
  getCustomerInfoByEmail(customer_email: string): Observable<any> {

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.getToken()}`
    });
    return this.http.get<any>(`${this.baseUrl}/GetCustomerByEmail?customer_email=${customer_email}`, { headers }).pipe(
      tap(response => {
     
      })
    );
  }
  getFeedToken(customer_email: string): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.getToken()}`
    });
    return this.http.get<any>(`${this.baseUrl}/GetFeedToken?email=${customer_email}`, { headers }).pipe(
      tap(response => {
        //  console.log('Received customer response:', response); // Log the response from the server
      })
    );
  }
  setEmail(customer_email: string): void {
    this.cookieService.set(this.emailSaved, customer_email, { path: '/' }); // Save email in cookie
  }

  // Method to update customer info
  updateCustomerInfo(updatedInfo: any, targetemail: string): Observable<any> {
    const token = this.getToken();

    if (!token) {
      console.error('No token available');
      // Handle token absence appropriately (return an observable with an error message, for example)
      return throwError(() => new Error('No token available'));
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const updateUrl = `${this.baseUrl}/UpdateCustomer?targetemail=${encodeURIComponent(targetemail)}`;
    console.log('Authorization Header Token:', headers.get('Authorization'));

    // Ensure updatedInfo matches CustomerModel exactly
    const completeInfo = {
      customer_firstname: updatedInfo.customer_firstname,
      customer_lastname: updatedInfo.customer_lastname,
      customer_email: updatedInfo.customer_email,
      customer_password: updatedInfo.customer_password,
      feed_token: updatedInfo.feed_token
    };

    return this.http.put<any>(updateUrl, completeInfo, { headers }).pipe(
      tap(response => {
        console.log('Update successful:', response);
      }),
      catchError(error => {
        console.error('Customer service update failed:', error);
        // You can return a meaningful error here, if needed
        return throwError(() => new Error('Error updating customer'));
      })
    );
  }
  private apiUrl = 'https://transit.land/api/v2/rest/agencies?adm0_name=Mexico&apikey=Z2xK57toXiR4t1cLlMvfC4fofM4ZhmVV';
  getAgencies(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }


  validateFeedToken(feed_token: string): Observable<any> {
    const token = this.getToken();

    if (!token) {
      console.error('No token available');
      // Handle token absence appropriately (return an observable with an error message, for example)
      return throwError(() => new Error('No token available'));
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const updateUrl = `${this.baseUrl}/ValidateFeedToken?feed_token=${feed_token}`;
    console.log(feed_token);
    return this.http.post<any>(`${this.baseUrl}/ValidateFeedToken?feed_token=${feed_token}`, { headers }).pipe(
      tap(response => {
        //  console.log('Received customer response:', response); // Log the response from the server
      })
    );

  }

  
  getEmailSaved(): string {
    return this.cookieService.get(this.emailSaved);

  }
  isLoggedIn(): boolean {
    console.log("isLoggedIn: ", this.isAuthenticated);
    return this.getToken() != null;  // Check directly if token exists
  }
  logout(): void {
    this.isAuthenticated = false;
    this.token = null;
    this.cookieService.delete(this.tokenSaved, '/');
    this.cookieService.delete(this.emailSaved, '/');
  }
}
