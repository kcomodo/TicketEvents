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
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { GuideComponent } from './components/guide/guide.component';
import { DiscoverComponent } from './components/discover/discover.component';
import { RegisterComponent } from './components/register/register.component';
import { TransitTrackerComponent } from './components/transit-tracker/transit-tracker.component';
import { ForgotUserComponent } from './components/forgot-user/forgot-user.component';
import { EmailConfirmationComponent } from './components/email-confirmation/email-confirmation.component';
import { MapComponent } from './components/map/map.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';


@NgModule({ declarations: [
        AppComponent,
        HomecomponentComponent,
        LogincomponentComponent,
        ProfilecomponentComponent,
        AboutcomponentComponent,
        GuideComponent,
        DiscoverComponent,
        RegisterComponent,
        TransitTrackerComponent,
        ForgotUserComponent,
        EmailConfirmationComponent,
        MapComponent,
        DashboardComponent
    ],
    bootstrap: [AppComponent], imports: [FormsModule,
        ReactiveFormsModule,
        BrowserModule,
        AppRoutingModule,
        RouterModule.forRoot([]) // add your routes here if any
    ], providers: [CustomerserviceService, CookieService, provideHttpClient(withInterceptorsFromDi())] })
export class AppModule { }
