import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomecomponentComponent } from './components/homecomponent/homecomponent.component';
import { LogincomponentComponent } from './components/logincomponent/logincomponent.component';
import { ProfilecomponentComponent } from './components/profilecomponent/profilecomponent.component';
import { AboutcomponentComponent } from './components/aboutcomponent/aboutcomponent.component';
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
    path: 'profile',
    component:ProfilecomponentComponent
  },
  {
    path: 'about',
    component:AboutcomponentComponent
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
