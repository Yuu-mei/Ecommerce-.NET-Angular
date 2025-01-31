import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Order } from '../videogame-order/types/Order';

@Injectable({
  providedIn: 'root'
})
export class VideogameOrderService {
  rest_service_route:string = "/server";

  constructor(private http:HttpClient) {}

  registerOrder(order:Order):Observable<string>{
    return this.http.post<string>(`${this.rest_service_route}/rest/register_order`, order);
  }
}
