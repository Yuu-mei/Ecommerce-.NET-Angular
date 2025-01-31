import { Component, Input } from '@angular/core';
import { Videogame } from '../types/videogame';
import { Router } from '@angular/router';

@Component({
  selector: 'app-videogame-card',
  standalone: true,
  imports: [],
  templateUrl: './videogame-card.component.html',
  styleUrl: './videogame-card.component.css'
})
export class VideogameCardComponent {
  @Input({required:true}) videogame!:Videogame;

  constructor(private router:Router){}

  getVideogameDetails(videogame_id:number){
    this.router.navigate(["videogame", videogame_id]);
  }
}
