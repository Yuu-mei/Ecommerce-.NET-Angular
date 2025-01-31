import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { appUser } from '../types/appUser';
import Swal from 'sweetalert2';
import { JwtService } from '../services/jwt.service';

@Component({
  selector: 'app-login-admin',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './login-admin.component.html',
  styleUrl: './login-admin.component.css'
})
export class LoginAdminComponent {
  user:appUser = {
    username: "",
    password: ""
  }

  constructor(private router:Router, private authService:AuthService, private jwtService:JwtService){}

  onLoginSubmit(loginForm: NgForm){
    this.authService.adminLogIn(this.user).subscribe(res => {
      if(res == "error"){
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Incorrect username and/or password",
        });
        return;
      }
      this.jwtService.storeToken(res);
      this.router.navigate(["/admin/videogame-list"]);
    });
  }
}
