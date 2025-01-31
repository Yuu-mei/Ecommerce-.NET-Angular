import { Component, OnInit } from '@angular/core';
import { OrderDetailRespAdmin } from '../../videogame-order/types/OrderDetailRespAdmin';
import { AdminService } from '../services/admin.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-admin-detail-order',
  standalone: true,
  imports: [],
  templateUrl: './admin-detail-order.component.html',
  styleUrl: './admin-detail-order.component.css'
})
export class AdminDetailOrderComponent implements OnInit{
  orderDetail:OrderDetailRespAdmin = {};
  order_id:number = -1;

  constructor(private adminService:AdminService, private activatedRoute:ActivatedRoute) {}

  ngOnInit(): void {
    // Reactive way to get the order_id
    this.activatedRoute.paramMap.subscribe({
      next: (params) => {
        this.order_id = Number(params.get("id"));
        this.getOrderDetails(this.order_id);
      },
      error: (err) => {
        console.error(`Error loading current order with id: ${this.order_id}`, err);
      }
    });
  }

  getOrderDetails(order_id:number){
    this.adminService.getOrderDetail(order_id).subscribe({
      next: (order:OrderDetailRespAdmin) => {
        this.orderDetail = order;
        console.log("Order", this.orderDetail);
      },
      error: (err) => {
        console.log(`Error loading the details of the order:`, err);
      }
    });
  }
}
