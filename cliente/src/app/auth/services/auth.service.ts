import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { appUser } from '../types/appUser';
import { ProfileInfo } from '../types/profileInfo';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private rest_service_route:string = "/server";
  // The !! operators converts a value into a boolean, easier to deal than with null/string
  private isLoggedIn:BehaviorSubject<boolean> = new BehaviorSubject<boolean>(!!localStorage.getItem("user"));
  isLoggedIn$:Observable<boolean> = this.isLoggedIn.asObservable();
  //Admin token check
  private isAdminLoggedIn:BehaviorSubject<boolean> = new BehaviorSubject<boolean>(!!localStorage.getItem("jwtadmin"));
  isAdminLoggedIn$:Observable<boolean> = this.isAdminLoggedIn.asObservable();
  
  constructor(private http:HttpClient) {}
  
  signUp(user:appUser, profile_pic:File):Observable<string>{
    const formData = new FormData();
    formData.append("username", user.username!);
    formData.append("email", user.email!);
    formData.append("password", user.password!);
    formData.append("profile_pic", profile_pic);

    return this.http.post<string>(`${this.rest_service_route}/rest/register_user`, formData);
  }

  logIn(user:appUser):Observable<any>{
    return this.http.post<appUser|string>(`${this.rest_service_route}/rest/login`, {
      "email": user.email,
      "password": user.password
    });
  }

  adminLogIn(user:appUser):Observable<string>{
    return this.http.post<string>(`${this.rest_service_route}/admin/login`, {
      "username": user.username,
      "password": user.password
    });
  }

  updateLogInObservable(user_id:string):void{
    localStorage.setItem("user", user_id);
    this.isLoggedIn.next(true);
  }

  updateAdminObservable():void{
    this.isAdminLoggedIn.next(true);
  }

  logOut(){
    //Simulation
    localStorage.removeItem("user");
    this.isLoggedIn.next(false);
  }

  retrieveUserData(user_id:string):Observable<ProfileInfo>{
    return this.http.get<ProfileInfo>(`${this.rest_service_route}/rest/get_user_data`, {
      params: {
        user_id
      }
    });
  }
}
