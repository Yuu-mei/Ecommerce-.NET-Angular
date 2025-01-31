import { Component, OnInit } from '@angular/core';
import { OrderRespAdmin } from '../../videogame-order/types/OrderRespAdmin';
import { AdminService } from '../services/admin.service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-admin-order-list',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './admin-order-list.component.html',
  styleUrl: './admin-order-list.component.css'
})
export class AdminOrderListComponent implements OnInit{
  orders:OrderRespAdmin[] = [];

  constructor(private adminService:AdminService, private router:Router) {}

  ngOnInit(): void {
    this.getAllOrders();
  }

  getAllOrders(){
    this.adminService.getAllOrders().subscribe({
      next: (orders:OrderRespAdmin[]) => {
        this.orders = orders;
        console.log(this.orders);
      },
      error: (err) => {
        console.log(`Error loading orders:`, err);
      }
    });
  }
}
