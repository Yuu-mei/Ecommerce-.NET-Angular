import { inject } from '@angular/core';
import { CanActivateChildFn, Router } from '@angular/router';
import { JwtService } from '../services/jwt.service';

export const adminGuard: CanActivateChildFn = (childRoute, state) => {
  const router = inject(Router);
  const jwtService = inject(JwtService);

  if(jwtService.isTokenExpired()){
    jwtService.logout();
    router.navigate(["/login-admin"], { queryParams: { returnUrl: state.url } });
    return false;
  }

  return true;
};
