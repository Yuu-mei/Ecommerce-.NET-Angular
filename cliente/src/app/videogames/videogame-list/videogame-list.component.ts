import { Component } from '@angular/core';
import { Videogame } from '../types/videogame';
import { VideogameService } from '../../services/videogame.service';
import { Router } from '@angular/router';
import { VideogameCardComponent } from "../videogame-card/videogame-card.component";
import { VideogameCarouselComponent } from "../videogame-carousel/videogame-carousel.component";

@Component({
  selector: 'app-videogame-list',
  standalone: true,
  imports: [
    VideogameCardComponent,
    VideogameCarouselComponent
],
  templateUrl: './videogame-list.component.html',
  styleUrl: './videogame-list.css',
})
export class VideogameListComponent {
  videogames:Videogame[] = [];
  latestVideogames:Videogame[] = [];

  loadingVideogames:boolean = true;
  loadingLatest:boolean = true;
  errorLoadingVideogames:boolean = false;
  errorLatest:boolean = false;

  constructor(private videogameService:VideogameService, private router:Router){}

  ngOnInit():void{
    //Latest videogames
    this.videogameService.obtainLatestVideogames().subscribe({
      next: (latest:Videogame[]) => {
        this.latestVideogames = latest;
      },
      error: (err) => {
        console.log(`Error loading latest videogames:`, err);
        this.errorLatest = true;
        this.loadingLatest = false;
      },
      complete: () => {
        this.loadingLatest = false;
      }
    });

    // All videogames
    this.videogameService.obtainAllVideogames().subscribe({
      next: (videogames:Videogame[]) => {
        this.videogames = videogames;
      },
      error: (err) => {
        console.log(`Error loading videogames:`, err);
        this.errorLoadingVideogames = true;
        this.loadingVideogames = false;
      },
      complete: () => {
        this.loadingVideogames = false;
      }
    });
  }

  getVideogameDetails(videogame_id:number){
    this.router.navigate(["videogame", videogame_id]);
  }
}
