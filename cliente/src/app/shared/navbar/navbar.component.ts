import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { VideogameService } from '../../services/videogame.service';
import { AuthService } from '../../auth/services/auth.service';
import { FormsModule, NgForm } from '@angular/forms';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    RouterLink,
    FormsModule
  ],
  templateUrl: './navbar.component.html',
  styles: '.active-page{color:red !important;}',
})
export class NavbarComponent implements OnInit{
  tags:string[] = [];
  isUserLoggedIn:boolean = false;
  isAdminLoggedIn:boolean = false;
  title:string = "";

  constructor(private router:Router, private videogameService:VideogameService, private authService:AuthService){}

  ngOnInit(): void {
    this.videogameService.obtainAllTags().subscribe(tags => {
      this.tags = tags;
    });
    // Subscribe to changes on the local storage
    this.authService.isLoggedIn$.subscribe((logged) => {
      this.isUserLoggedIn = logged;
    });
    this.authService.isAdminLoggedIn$.subscribe((logged) => {
      this.isAdminLoggedIn = logged;
    });
  }

  getVideogamesByTag(tag:string){
    this.router.navigate(['search'], {
      queryParams: {
        tag
      }
    });
  }

  logOut(event:MouseEvent){
    event.preventDefault();
    Swal.fire({
      title: "Logged out successfully!",
      icon: "success"
    });
    this.router.navigate(['']);
    this.authService.logOut();
  }

  searchGame(searchForm:NgForm){
    this.router.navigate(['search'], {
      queryParams: {
        title: this.title
      }
    });
  }

  advancedSearch(){
    this.router.navigate(['advanced_search']);
  }

}
