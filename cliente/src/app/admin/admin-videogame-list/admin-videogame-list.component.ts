import { Component, OnInit } from '@angular/core';
import { Videogame } from '../../videogames/types/videogame';
import { Router, RouterLink } from '@angular/router';
import { AdminService } from '../services/admin.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-admin-videogame-list',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './admin-videogame-list.component.html',
  styleUrl: './admin-videogame-list.component.css'
})
export class AdminVideogameListComponent implements OnInit{
  videogames:Videogame[] = [];

  constructor(private adminService:AdminService, private router:Router){}

  ngOnInit(): void {
    this.getAllVideogames();
  }

  getAllVideogames(){
    this.adminService.obtainAllVideogames().subscribe({
      next: (videogames:Videogame[]) => {
        this.videogames = videogames;
      },
      error: (err) => {
        console.log(`Error loading videogames:`, err);
      }
    });
  }

  deleteVideogame(id:number){
    this.adminService.deleteVideogame(id).subscribe(res => {
      if(res === "ok"){
        Swal.fire({
          title: "Game added!",
          icon: "success"
        });
        this.getAllVideogames();
      }else{
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Something went wrong",
        });
      }
    })
  }

  editVideogame(id:number){
    this.router.navigate([`admin/edit-videogame/${id}`]);
  }

  deactivateVideogame(id:number){
    this.adminService.deactivateVideogame(id).subscribe(res => {
      if(res == "ok"){
        Swal.fire({
          title: "Game deactivated!",
          icon: "success"
        });
      }else{
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Something went wrong",
        });
      }
    })
  }
}
