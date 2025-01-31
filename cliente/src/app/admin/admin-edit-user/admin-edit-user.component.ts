import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { appUser } from '../../auth/types/appUser';
import { AdminService } from '../services/admin.service';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-admin-edit-user',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './admin-edit-user.component.html',
  styleUrl: './admin-edit-user.component.css'
})
export class AdminEditUserComponent implements OnInit{
  user:appUser = {};
  user_image:File = {} as File;
  user_id:number = -1;

  constructor(private adminService:AdminService, private router:Router ,private activatedRoute:ActivatedRoute) {}

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe({
      next: (params) => {
        this.user_id = Number(params.get("id"));
        this.getUserById(this.user_id);
      }
    })
  }

  getUserById(user_id:number){
    this.adminService.getUserById(user_id).subscribe((res) => {
      this.user = res;
    });
  }

  profilePicChange(event:Event){
    const input = event.target as HTMLInputElement;
    this.user_image = input.files![0];
  }

  onActiveChange(event:Event){
    const input = event.target as HTMLInputElement;
    this.user.active = input?.checked ? 1 : 0;
  }

  onEditFormSubmit(editForm:NgForm){
    const formData = new FormData();
    let active = this.user.active === 1 ? "true" : "false";

    formData.append("profile_pic", this.user_image);
    formData.append("username", this.user.username!);
    formData.append("email", this.user.email!);
    formData.append("password", this.user.password!);
    formData.append("active", active);
    formData.append("user_id", this.user_id.toString());

    this.adminService.editUser(formData).subscribe((res) => {
      if (res == "OK"){
        Swal.fire({
          title: "User edited",
          icon: "success"
        });
        this.router.navigate(["admin/user-list"]);
      }else {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Something went wrong editing your account",
        });
      }
    })
  }
}
