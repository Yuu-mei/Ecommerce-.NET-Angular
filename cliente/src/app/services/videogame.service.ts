import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Videogame } from '../videogames/types/videogame';
import { Filters } from '../search/types/filters';

@Injectable({
  providedIn: 'root'
})
export class VideogameService {
  rest_service_route:string = "/server";

  constructor(private http:HttpClient) {}

  obtainAllVideogames():Observable<Videogame[]>{
    return this.http.get<Videogame[]>(`${this.rest_service_route}/rest/get_all_videogames`);
  }

  obtainAllTags():Observable<string[]>{
    return this.http.get<string[]>(`${this.rest_service_route}/rest/get_all_tags`);
  }

  obtainLatestVideogames():Observable<Videogame[]>{
    return this.http.get<Videogame[]>(`${this.rest_service_route}/rest/get_latest_videogames`);
  }

  obtainSimilarVideogames(videogame_id:number, developer:string, tag:string):Observable<Videogame[]>{
    return this.http.get<Videogame[]>(`${this.rest_service_route}/rest/get_similar_videogames`, {
      params: {
        videogame_id,
        developer,
        tag
      }
    });
  }

  obtainVideogameDetails(videogame_id:number):Observable<Videogame>{
    return this.http.get<Videogame>(`${this.rest_service_route}/rest/get_videogame_detail`, {
      params: {
        id: videogame_id
      }
    });
  }

  searchVideogames(params:any):Observable<Videogame[]>{
    return this.http.get<Videogame[]>(`${this.rest_service_route}/rest/search`, {
      params: params
    });
  }

  obtainFilters():Observable<Filters>{
    return this.http.get<Filters>(`${this.rest_service_route}/rest/get_filters`);
  }

  wishlistVideogame(videogame_id:number, user_id:number):Observable<string>{
    return this.http.post<string>(`${this.rest_service_route}/rest/wishlist_videogame`, {
      videogame_id: videogame_id.toString(),
      user_id: user_id.toString()
    });
  }

  isVideogameWishlisted(videogame_id:number, user_id:number):Observable<string>{
    return this.http.post<string>(`${this.rest_service_route}/rest/is_wishlisted`, {
      videogame_id: videogame_id.toString(),
      user_id: user_id.toString()
    });
  }
}
