import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';  // Used to inject services in standalone functions
import { CustomerserviceService } from '../service/customerservice.service';  
import { Router } from '@angular/router';

export const accessGuard: CanActivateFn = (route, state) => {
  const authenticate = inject(CustomerserviceService);  // Inject the AuthService
  const router = inject(Router);  // Inject the Router

  if (authenticate.isLoggedIn()) {
    return true;  // Allow access to the route
  } else {
    router.navigate(['/login']);  // Redirect to the login page if not authenticated
    return false;  // Block access to the route
  }
};
