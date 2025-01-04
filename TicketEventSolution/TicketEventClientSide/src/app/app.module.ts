import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
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
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatStepperModule } from '@angular/material/stepper';
import { MatButtonModule } from '@angular/material/button';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';


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
      MatButtonModule,
      MatStepperModule,
      FormsModule,
      MatFormFieldModule,
      BrowserAnimationsModule,
      MatInputModule,
        RouterModule.forRoot([]), // add your routes here if any

        
    ], providers: [CustomerserviceService, CookieService, provideHttpClient(withInterceptorsFromDi()), provideAnimationsAsync()] })
export class AppModule { }
