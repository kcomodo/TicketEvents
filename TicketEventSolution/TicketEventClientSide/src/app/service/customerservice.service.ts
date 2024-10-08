import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service'; //npm install ngx-cookie-service --save --force used for read, saet, and delete browser cookies
import { tap } from 'rxjs/operators';
import { Token } from '@angular/compiler';
import { map } from 'rxjs/operators';
import { response } from 'express';
import { HttpHeaders } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { Observable, empty } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class CustomerserviceService {
  //default url for the api which is swagger
  private token: string | null = null;
  private isAuthenticated = false;
  private baseUrl = "http://localhost:7240";
  private tokenSaved = 'tokenSaved';
  constructor(private http: HttpClient, private cookieService: CookieService) {
    const token = this.getToken();
    const header = new HttpHeaders({})
  }
  validateLogin(email: string, password: string) //: Observable<boolean>
  {
    const url = `${this.baseUrl}/ValidateLogin?email=${encodeURIComponent(email)}&password=${encodeURIComponent(password)}`;
    return this.http.post<{ token: string }>(`${this.baseUrl}/ValidateLogin?email=${email}&password=${password}`, "")
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
