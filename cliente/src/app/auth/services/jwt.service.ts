import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class JwtService {
  constructor(private router:Router, private authService:AuthService) { }

  storeToken(jwt:string):void{
    localStorage.setItem("jwtadmin", jwt);
    this.authService.updateAdminObservable();
  }

  getToken():any{
    return localStorage.getItem("jwtadmin");
  }

  decodeToken(){
    const jwt = this.getToken();
    return jwt ? jwtDecode(jwt) : null;
  }

  isTokenExpired():boolean{
    const jwt = this.getToken();
    if(!jwt) return true;

    const decodedJWT = this.decodeToken();
    if(!decodedJWT || !decodedJWT.exp) return true;

    const currTime = Math.floor(Date.now() / 1000);
    return decodedJWT.exp < currTime;
  }

  logout():void{
    localStorage.removeItem("jwtadmin");
    this.router.navigate(["../login-admin"]);
  }
}
