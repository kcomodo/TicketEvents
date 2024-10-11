import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomecomponentComponent } from './components/homecomponent/homecomponent.component';
import { LogincomponentComponent } from './components/logincomponent/logincomponent.component';
import { ProfilecomponentComponent } from './components/profilecomponent/profilecomponent.component';
import { AboutcomponentComponent } from './components/aboutcomponent/aboutcomponent.component';
import { CustomerserviceService } from './service/customerservice.service';
import { RouterModule } from '@angular/router'; // for routing
import { CookieService } from 'ngx-cookie-service';
import { HttpClientModule } from '@angular/common/http';
import { RegisterComponent } from './components/register/register.component';
import { GuideComponent } from './components/guide/guide.component';
import { DiscoverComponent } from './components/discover/discover.component';
@NgModule({
  declarations: [
    AppComponent,
    HomecomponentComponent,
    LogincomponentComponent,
    ProfilecomponentComponent,
    AboutcomponentComponent,
    RegisterComponent,
    GuideComponent,
    DiscoverComponent,
  ],
  imports: [
    FormsModule,
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    RouterModule.forRoot([]) // add your routes here if any

  ],
  providers: [CustomerserviceService, CookieService],
  bootstrap: [AppComponent]
})
export class AppModule { }
