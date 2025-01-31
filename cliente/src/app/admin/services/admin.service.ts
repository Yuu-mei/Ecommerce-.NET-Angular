import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { JwtService } from '../../auth/services/jwt.service';
import { Videogame } from '../../videogames/types/videogame';
import { appUser } from '../../auth/types/appUser';
import { OrderRespAdmin } from '../../videogame-order/types/OrderRespAdmin';
import { OrderDetailRespAdmin } from '../../videogame-order/types/OrderDetailRespAdmin';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private rest_service_route:string = "/server";

  constructor(private http:HttpClient, private jwtService:JwtService) { }

  getHeaders():HttpHeaders{
    const jwt = this.jwtService.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${jwt}`
    });

    return headers;
  }

  addVideogame(formData:FormData):Observable<string>{
    const headers = this.getHeaders();
    return this.http.post<string>(`${this.rest_service_route}/admin/add_videogame`, formData, {headers});
  }

  editVideogame(formData:FormData):Observable<string>{
    const headers = this.getHeaders();
    return this.http.post<string>(`${this.rest_service_route}/admin/edit_videogame`, formData, {headers});
  }

  obtainAllVideogames():Observable<Videogame[]>{
    const headers = this.getHeaders();
    return this.http.get<Videogame[]>(`${this.rest_service_route}/admin/get_all_videogames_admin`, {headers});
  }

  deleteVideogame(id:number):Observable<string>{
    const headers = this.getHeaders();
    return this.http.get<string>(`${this.rest_service_route}/admin/delete_videogame`, {
      params:{
        "id": id
      },
      headers
    }); 
  }

  deactivateVideogame(id:number):Observable<string>{
    const headers = this.getHeaders();
    return this.http.get<string>(`${this.rest_service_route}/admin/deactivate_videogame`, {
      params: {
        id
      },
      headers
    })
  }

  getAllUsers():Observable<appUser[]>{
    const headers = this.getHeaders();
    return this.http.get<appUser[]>(`${this.rest_service_route}/admin/get_all_users`, {
      headers
    });
  }

  getUserById(user_id:number):Observable<appUser>{
    const headers = this.getHeaders();
    return this.http.get<appUser>(`${this.rest_service_route}/admin/get_user_by_id`, {
      params: {
        user_id
      },
      headers
    });
  }

  getVideogameById(videogame_id:number):Observable<Videogame>{
    const headers = this.getHeaders();
    return this.http.get<Videogame>(`${this.rest_service_route}/admin/get_videogame_by_id`, {
      params: {
        videogame_id
      },
      headers
    });
  }

  addUser(user:appUser, active:string, profile_pic:File):Observable<string>{
    const headers = this.getHeaders();
    const formData = new FormData();
    formData.append("username", user.username!);
    formData.append("email", user.email!);
    formData.append("password", user.password!);
    formData.append("active", active);
    formData.append("profile_pic", profile_pic);

    return this.http.post<string>(`${this.rest_service_route}/admin/register_user`, formData, {headers});
  }

  editUser(formData:FormData):Observable<string>{
    const headers = this.getHeaders();
    return this.http.post<string>(`${this.rest_service_route}/admin/edit_user`, formData, {headers});
  }

  deactivateUser(user_id:number):Observable<string>{
    const headers = this.getHeaders();
    return this.http.get<string>(`${this.rest_service_route}/admin/deactivate_user`, {
      params:{
        user_id
      },
      headers
    });
  }

  getAllOrders():Observable<OrderRespAdmin[]>{
    const headers = this.getHeaders();
    return this.http.get<OrderRespAdmin[]>(`${this.rest_service_route}/admin/get_all_orders`, {
      headers
    });
  }

  getOrderDetail(order_id:number):Observable<OrderDetailRespAdmin>{
    const headers = this.getHeaders();
    return this.http.get<OrderDetailRespAdmin>(`${this.rest_service_route}/admin/get_order_detail`, {
      params:{
        order_id
      },
      headers
    });
  }
}
