import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CartItem } from '../types/cartItem';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  rest_service_route:string = "/server";

  constructor(private http:HttpClient) {}

  retrieveCartItems(user_id:string):Observable<CartItem[]>{
    return this.http.get<CartItem[]>(`${this.rest_service_route}/rest/get_cart_products`, {
      params: {
        user_id
      }
    });
  }

  // As of right now, this method also updates the quantity as you pass the quantity to it and it checks if it is already on the cart to update it
  addVideogameToCart(videogame_id: number, user_id:string, quantity:number):Observable<string> {
    return this.http.post<string>(`${this.rest_service_route}/rest/add_cart_product`, {
      videogame_id: videogame_id.toString(),
      quantity: quantity,
      user_id: user_id
    });
  }

  removeVideogameFromCart(videogame_id: number, user_id:string):Observable<void> {
    return this.http.post<void>(`${this.rest_service_route}/rest/remove_cart_item`, {
      videogame_id: videogame_id.toString(),
      user_id: user_id
    });
  }
}
