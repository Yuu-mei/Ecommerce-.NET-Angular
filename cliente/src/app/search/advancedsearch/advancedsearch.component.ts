import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { SearchParams } from '../types/searchParams';
import { VideogameService } from '../../services/videogame.service';
import { Filters } from '../types/filters';

@Component({
  selector: 'app-advancedsearch',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './advancedsearch.component.html',
  styleUrl: './advancedsearch.component.css'
})
export class AdvancedSearchComponent implements OnInit{
  searchParams:SearchParams = {
    title: '',
    developer: '',
    tag: ''
  };
  filters:Filters = {
    devs: [],
    tags: []
  };

  ngOnInit(): void {
    this.videogameService.obtainFilters().subscribe((res) => {
      this.filters = res;
    });
  }

  constructor(private router:Router, private videogameService:VideogameService) {}

  searchGame(searchForm:NgForm){
    this.router.navigate(["search"], {
      queryParams: {
        title: this.searchParams.title,
        tag: this.searchParams.tag,
        developer: this.searchParams.developer
      }
    })
  }
}
