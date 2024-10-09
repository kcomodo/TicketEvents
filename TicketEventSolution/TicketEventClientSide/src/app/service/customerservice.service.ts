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
export class CustomerserviceService {
  
  //default url for the api which is swagger
  private token: string | null = null;
  private isAuthenticated = false;
  private baseUrl = "https://localhost:7240/api/Values";
  private tokenSaved = 'tokenSaved';
  constructor(private http: HttpClient, private cookieService: CookieService) {
  }
  
  validateLogin(email: string, password: string): Observable<boolean>
  {
    console.log("customer service now: ", email, password);
    const url = `${this.baseUrl}/ValidateLogin?email=${encodeURIComponent(email)}&password=${encodeURIComponent(password)}`;
    //keep it as a post
    return this.http.post<{ token: string }>(`${this.baseUrl}/ValidateLogin?email=${email}&password=${password}`, "")
      .pipe(
       
        tap(response => {
          console.log(response);
        if (response.token) {
          this.token = response.token;
          this.isAuthenticated = true; // Set authentication status based on response
            console.log("isLoggedin received: ", this.isAuthenticated);
           console.log("Token received and saved: ", this.token);
          this.cookieService.set(this.tokenSaved, this.token, { path: '/' });
        }
        else if (this.getToken() != null) {
          this.isAuthenticated = true;
          console.log("token is not null");
          console.log("token not null and: ", email,password)
        }
        else {
          this.isAuthenticated = false;
            console.log(response.token);
            console.log("isLoggedin failed: ", this.isAuthenticated);
        }

      }),

      map(response => !!response.token) // Transform response to a boolean
    );

  
  }
  registerCustomer() {

  }
  updateCustomerInfo() {

  }
  
  getToken(): string | null {
    //grab the token from the cookie service and then return it to be stored
    //cookieService is a library that allows us to store the token
   return this.cookieService.get(this.tokenSaved) || null;
  }
  
}
