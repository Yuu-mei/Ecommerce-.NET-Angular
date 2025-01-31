import { Component } from '@angular/core';
import { appUser } from '../../auth/types/appUser';
import { AuthService } from '../../auth/services/auth.service';
import { Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import Swal from 'sweetalert2';
import { AdminService } from '../services/admin.service';

@Component({
  selector: 'app-admin-add-user',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './admin-add-user.component.html',
  styleUrl: './admin-add-user.component.css'
})
export class AdminAddUserComponent {
  user:appUser = {};
  active:string = "false";
  profile_pic!:File;

  constructor(private adminService:AdminService, private router:Router){}

  onFileChange(event:any){
    this.profile_pic = event.target.files[0];
  }

  onSignUpForm(signUpForm: NgForm){
    this.adminService.addUser(this.user, this.active, this.profile_pic).subscribe({
      next: (res) => {
        console.log(res);
        if(res === "ok"){
          this.router.navigate(["admin/user-list"])
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

  onSaleChange(event:Event){
    const input = event.target as HTMLInputElement;
    this.active = input?.checked ? "true" : "false";
  }
}
