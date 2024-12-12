import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomecomponentComponent } from './components/homecomponent/homecomponent.component';
import { LogincomponentComponent } from './components/logincomponent/logincomponent.component';
import { ProfilecomponentComponent } from './components/profilecomponent/profilecomponent.component';
import { AboutcomponentComponent } from './components/aboutcomponent/aboutcomponent.component';
import { GuideComponent } from './components/guide/guide.component';
import { DiscoverComponent } from './components/discover/discover.component';
import { RegisterComponent }from './components/register/register.component';
import { accessGuard } from './guards/access.guard';
import { TransitTrackerComponent } from './components/transit-tracker/transit-tracker.component';
import { ForgotUserComponent } from './components/forgot-user/forgot-user.component';
import { EmailConfirmationComponent } from './components/email-confirmation/email-confirmation.component';
import { MapComponent } from './components/map/map.component';
const routes: Routes = [
  {
    path: 'login',
    component:LogincomponentComponent
  },
  {
    path: 'home',
    component:HomecomponentComponent
  },
  {
    path: 'map',
    component: MapComponent
  },
  {
    path: 'profile',
    component: ProfilecomponentComponent,
    canActivate: [accessGuard]
  },
  {
    path: 'transitEvent',
    component: TransitTrackerComponent,
    canActivate: [accessGuard]
  },
  {
    path: 'about',
    component:AboutcomponentComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'guide',
    component: GuideComponent
  },
  {
    path: 'discover',
    component: DiscoverComponent
  },
  {
    path: 'forgot',
    component: ForgotUserComponent
  },
  {
    path: 'confirmation',
    component: EmailConfirmationComponent,
    //canActivate: [accessGuard]
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch:'full'
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
