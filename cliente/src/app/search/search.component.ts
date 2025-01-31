import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { VideogameService } from '../services/videogame.service';
import { Videogame } from '../videogames/types/videogame';
import { SearchParams } from './types/searchParams';
import { VideogameCardComponent } from '../videogames/videogame-card/videogame-card.component';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [
    VideogameCardComponent,
  ],
  templateUrl: './search.component.html',
  styleUrl: './search.component.css'
})
export class SearchComponent implements OnInit{
  searchParams:SearchParams = {};
  searchResults:Videogame[] = [];
  paramsError:boolean = false;
  isLoading:boolean = true;

  constructor(private activatedRoute:ActivatedRoute, private videogameService:VideogameService){}

  ngOnInit(): void {
    // Complete never happens on queryparams so you have to use it on next
    this.activatedRoute.queryParams.subscribe({
      next: (params) => {
        this.searchParams = params;
        if(Object.keys(this.searchParams).length === 0){
          console.error('Error loading params, please check the url');
          this.paramsError = true;
          this.isLoading = false;
          return;
        }
        this.search(this.searchParams);
      },
      error: (err) => {
        console.error(`Error loading params: ${err}`);
        this.paramsError = true;
        this.isLoading = false;
      }
    });
  }

  search(params:any){
    this.videogameService.searchVideogames(params).subscribe({
      next: (videogames) => {
        this.searchResults = videogames;
      },
      error: (err) => {
        console.error(`Error returning videogames from search: ${err}`);
        this.paramsError = true;
        this.isLoading = false;
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

}
