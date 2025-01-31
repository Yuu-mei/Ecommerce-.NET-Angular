import { Component, OnInit } from '@angular/core';
import { appUser } from '../../auth/types/appUser';
import { AdminService } from '../services/admin.service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-user-list',
  standalone: true,
  imports: [],
  templateUrl: './admin-user-list.component.html',
  styleUrl: './admin-user-list.component.css'
})
export class AdminUserListComponent implements OnInit {
  users:appUser[] = [];

  constructor(private adminService:AdminService, private router:Router) {}

  ngOnInit(): void {
      this.getAllUsers();
  }

  getAllUsers(){
    this.adminService.getAllUsers().subscribe({
      next: (users:appUser[]) => {
        this.users = users;
        console.log(this.users);
      },
      error: (err) => {
        console.log(`Error loading users:`, err);
      }
    });
  }

  editUser(user_id:number){
    this.router.navigate([`admin/edit-user/${user_id}`]);
  }

  deactivateUser(user_id:number){
    this.adminService.deactivateUser(user_id).subscribe({
      next: (res) => {
        if(res=="ok"){
          Swal.fire({
            title: "User deactivated successfully!",
            icon: "success"
          });
          this.getAllUsers();
        }
      },
      error: (err) => {
        console.log(`Error deactivating user:`, err);
      }
    })
  }
}
