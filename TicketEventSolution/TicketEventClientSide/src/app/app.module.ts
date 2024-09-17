import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomecomponentComponent } from './components/homecomponent/homecomponent.component';
import { LogincomponentComponent } from './components/logincomponent/logincomponent.component';
import { ProfilecomponentComponent } from './components/profilecomponent/profilecomponent.component';
import { AboutcomponentComponent } from './components/aboutcomponent/aboutcomponent.component';

@NgModule({
  declarations: [
    AppComponent,
    HomecomponentComponent,
    LogincomponentComponent,
    ProfilecomponentComponent,
    AboutcomponentComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
