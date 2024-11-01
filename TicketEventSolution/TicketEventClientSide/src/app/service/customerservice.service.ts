import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service'; //npm install ngx-cookie-service --save --force used for read, saet, and delete browser cookies
import { tap } from 'rxjs/operators';
import { Token } from '@angular/compiler';
import { map } from 'rxjs/operators';
import { response } from 'express';
import { HttpHeaders } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { Observable, empty } from 'rxjs';
import { Component, OnInit } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class CustomerserviceService implements OnInit{
  
  //default url for the api which is swagger
  private token: string | null = null;
  private isAuthenticated = false;
  private baseUrl = "https://localhost:7240/api/Values";
  private tokenSaved = 'tokenSaved';
  constructor(private http: HttpClient, private cookieService: CookieService) {
  }
  ngOnInit() {
    if (this.getToken() != null) {
      this.isAuthenticated = true;
      // console.log("the token is true, will stay on customer info page: ", this.isAuthenticated);
    }
  }
  validateLogin(email: string, password: string): Observable<boolean>
  {
    console.log("customer service now: ", email, password);
    //keep it as a post because the information is not visible to the URl, and is more secured
    //old method
    //return this.http.post<{ token: string }>(`${this.baseUrl}/ValidateLogin?email=${email}&password=${password}`, "")
    //new method, this is more secured instead of directly accessing the url, we are sending it into the body of the request
    return this.http.post<{ token: string }>(`${this.baseUrl}/ValidateLogin`, { email, password }, {
      headers: { 'Content-Type': 'application/json' }
    })
      .pipe(
        tap(response => {
          console.log(response);
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
  registerCustomer(firstname: string, lastname: string, email: string, password: string) {
    console.log("registerCustomer called");
    return this.http.post<any>(`${this.baseUrl}/AddCustomer`, { firstname, lastname, email, password}, {
      headers: { 'Content-Type': 'application/json' }
    })
  }
  /*
  updateCustomerInfo(updatedInfo: any): Observable<any> {
    //replace the return later
    return this.http.post<any>(`${this.baseUrl}/UpdateCustomer`, { updatedInfo.firstname, updatedInfo.lastname, updatedInfo.email, updatedInfo.password}, {
      headers: { 'Content-Type': 'application/json' }
    })
  }
*/  
  getToken(): string | null {
    //grab the token from the cookie service and then return it to be stored
    //cookieService is a library that allows us to store the token
   return this.cookieService.get(this.tokenSaved) || null;
  }
  // Need to decode token to recieve email
  /*
  getEmail(): Observable<any>{
    //replace this return later
    const body = {};
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.token}`
    });
    console.log("service token: ", this.token);
    console.log("Header:",headers);
    
    //return this.http.get<string>(`${this.baseUrl}/GetCustomerEmail?email=${this.token}`, body);
    return this.http.get<string>(`${this.baseUrl}/GetCustomerEmail`, { headers });
  }
  */
  getEmail(): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.token}`
    });
    console.log("Service token: ", this.token);
    console.log("Header:", headers);

    return this.http.get<string>(`${this.baseUrl}/GetCustomerEmail`, { headers });
  }
  
  isLoggedIn(): boolean {
    return this.isAuthenticated;
  }
  lougout(): void {
    this.isAuthenticated = false;
    this.token = null;
    this.cookieService.delete(this.tokenSaved, '/');
  }
}
