import { Component, Input } from '@angular/core';
import { Videogame } from '../types/videogame';
import { Router } from '@angular/router';

@Component({
  selector: 'app-videogame-carousel',
  standalone: true,
  imports: [],
  templateUrl: './videogame-carousel.component.html',
  styleUrl: './videogame-carousel.component.css'
})
export class VideogameCarouselComponent {
  @Input({required: true}) videogame!:Videogame;

  constructor(private router:Router) {}

  getVideogameDetails(videogame_id:number){
    this.router.navigate(["videogame", videogame_id]);
  }

  getVideogamesByDeveloper(developer:string){
    this.router.navigate(['search'], {
      queryParams: {
        developer
      }
    });
  }
}
