import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { appUser } from '../types/appUser';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  user:appUser = {};
  profile_pic!:File;

  constructor(private authService:AuthService, private router:Router){}

  onFileChange(event:any){
    this.profile_pic = event.target.files[0];
  }

  onSignUpForm(signUpForm: NgForm){
    this.authService.signUp(this.user, this.profile_pic).subscribe({
      next: (res) => {
        if(res === "ok"){
          this.router.navigate(["login"])
        }else{
          Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Something went wrong creating your account",
          });
          return;
        }
      }
    })
  }
}
