import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomecomponentComponent } from './homecomponent/homecomponent.component';
import { LogincomponentComponent } from './logincomponent/logincomponent.component';
import { ProfilecomponentComponent } from './profilecomponent/profilecomponent.component';
import { AboutcomponentComponent } from './aboutcomponent/aboutcomponent.component';
import { EventscomponentComponent } from './components/eventscomponent/eventscomponent.component';

@NgModule({
  declarations: [
    AppComponent,
    HomecomponentComponent,
    LogincomponentComponent,
    ProfilecomponentComponent,
    AboutcomponentComponent,
    EventscomponentComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
