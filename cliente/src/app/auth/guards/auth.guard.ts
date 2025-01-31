import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CartService } from '../../cart/services/cart.service';
import { AuthService } from '../services/auth.service';
import { take, map, switchMap, of } from 'rxjs';
import Swal from 'sweetalert2';

export const authGuard: CanActivateFn = () => {
  const router:Router = inject(Router);
  const authService:AuthService = inject(AuthService);

  // This is quite the simple guard, it should instead use a token like JWT and double check it with the server to see if it still operative or has been cancelled for some reason
  return authService.isLoggedIn$.pipe(
    take(1),
    map((logged) => {
      if(logged){
        return true;
      }else{
        Swal.fire({
          icon: "error",
          title: "Log in first!",
        });
        router.navigate(["login"]);
        return false;
      }
    })
  );
};

export const orderGuard: CanActivateFn = () => {
  const router:Router = inject(Router);
  const cartService:CartService = inject(CartService);
  const authService:AuthService = inject(AuthService);

  return authService.isLoggedIn$.pipe(
    take(1),
    switchMap((logged) => {
      if (!logged) {
        Swal.fire({
          icon: "error",
          title: "Log in first!",
        });
        router.navigate(["login"]);
        return of(false);
      }

      return cartService.retrieveCartItems(localStorage.getItem("user")!).pipe(
        take(1),
        map((res) => {
          if (res.length === 0) {
            Swal.fire({
              icon: "error",
              title: "Add items to your cart first!",
            });
            router.navigate([""]);
            return false;
          }
          return true;
        })
      );
    })
  );
}