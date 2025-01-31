import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { VideogameOrderService } from '../services/videogame-order.service';
import { Order } from './types/Order';
import { FormsModule, NgForm } from '@angular/forms';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-videogame-order',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './videogame-order.component.html',
  styleUrl: './videogame-order.component.css'
})
export class VideogameOrderComponent {
  videogame_order:Order = {
    FullName: '',
    Address: '',
    Country: '',
    State: '',
    ZipCode: '',
    CardNumber: '',
    CCV: 0,
    CardOwner: '',
    UserId: '',
    OrderDate: new Date().toISOString()
  }

  constructor(private router:Router, private orderService:VideogameOrderService){}


  processOrder(orderForm:NgForm){
    this.videogame_order.UserId = localStorage.getItem("user")!;
    console.log(this.videogame_order);
    this.orderService.registerOrder(this.videogame_order).subscribe((res) => {
      if(res === "ok"){
        Swal.fire({
          title: "Order confirmed!",
          text: "Thanks for buying!",
          icon: "success"
        });
        this.router.navigate([""]);
      }else{
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Something went wrong processing your order",
        });
      }
    });
  }
}
