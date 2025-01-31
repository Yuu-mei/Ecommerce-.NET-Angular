import { Component, OnInit } from '@angular/core';
import { CartService } from './services/cart.service';
import { CartItem } from './types/cartItem';
import { CartItemComponent } from './cart-item/cart-item.component';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    CartItemComponent
  ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit{
  cartItems:CartItem[] = [];
  totalPrice:number = 0;
  loading:boolean = true;
  error:boolean = false;

  constructor(private cartService:CartService, private router:Router){}

  ngOnInit(): void {
    if(localStorage.getItem("user")){
      this.cartService.retrieveCartItems(localStorage.getItem("user")!).subscribe({
        next: (res) => {
          this.cartItems = res;
          this.updateTotalPrice();
        },
        error: (err) => {
          console.error('Error loading cart items ',err);
          this.loading = false;
          this.error = true;
        },
        complete: () => {
          this.loading = false;
        }
      });
    }
  }

  updateTotalPrice():void{
    this.totalPrice = this.cartItems.reduce((sum, cartItem) => {
      return sum + cartItem.price * cartItem.quantity;
    }, 0);
    this.totalPrice = Number(this.totalPrice.toFixed(2));
  }

  onQuantityChange(cartItem:CartItem, quantity:number):void{
    cartItem.quantity = quantity;
    this.updateTotalPrice();
    //Quantity of that item in the backend has already been done in the cartitem, may be best to do it here
  }

  onRemove(cartItem:CartItem):void{
    this.cartItems = this.cartItems.filter((item) => item !== cartItem);
    this.updateTotalPrice();
    this.cartService.removeVideogameFromCart(cartItem.videogameId, localStorage.getItem("user")!).subscribe();
  }

  checkOut(){
    //Shouldn't be needed, it will be disabled if there are no items
    if(this.cartItems === undefined || this.cartItems.length === 0){
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "No products in cart",
      });
      return;
    }
    this.router.navigate(["orders"]);
  }
}
