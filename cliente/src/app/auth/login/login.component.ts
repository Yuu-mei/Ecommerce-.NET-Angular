import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { appUser } from '../types/appUser';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  user:appUser = {
    email: '',
    password: ''
  }

  constructor(private router:Router, private authService:AuthService){}

  onLoginSubmit(loginForm: NgForm){
    this.authService.logIn(this.user).subscribe((res) => {
      if(res == "error"){
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Incorrect email and/or password",
        });
        return;
      }
      this.authService.updateLogInObservable(res);
      Swal.fire({
        title: "Logged in successfully!",
        icon: "success"
      });
      this.router.navigate(["videogame-list"]);
    });
  }
}
