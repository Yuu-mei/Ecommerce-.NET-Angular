import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { ProfileInfo } from '../types/profileInfo';
import { Router } from '@angular/router';
import { VideogameCardComponent } from '../../videogames/videogame-card/videogame-card.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    VideogameCardComponent
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit{
  profile_info!:ProfileInfo;
  user_id:string = '';

  constructor(private authService:AuthService, private router:Router){}

  ngOnInit(): void {
    this.user_id = localStorage.getItem("user")!;
    this.authService.retrieveUserData(this.user_id).subscribe((res) => {
      this.profile_info = res;
      console.log(this.profile_info)
    });
  }

  getVideogameDetails(videogame_id:number, event:MouseEvent){
    event.preventDefault();
    this.router.navigate(["videogame", videogame_id]);
  }

}
