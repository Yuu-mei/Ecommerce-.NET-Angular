import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CartItem } from '../types/cartItem';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-cart-item',
  standalone: true,
  imports: [],
  templateUrl: './cart-item.component.html',
  styleUrl: './cart-item.component.css'
})
export class CartItemComponent implements OnInit{
  @Input({required:true}) cartItem!:CartItem;
  shortenedDescription:string = '';
  // And now to use Godot Signal-no, nevermind, it's Angular, it's EventEmitters
  @Output() onQuantityChange = new EventEmitter<number>();
  @Output() onRemove = new EventEmitter<void>();

  ngOnInit(): void {
    // Doing it here because otherwise it breaks since it does it BEFORE data loads
    this.shortenedDescription = this.cartItem.description!.slice(0, 40).concat("...");
  }

  constructor(private cartService:CartService) {}

  modifyQuantity(quantity:number){
    //This should not happen since I have it disabled but just to make sure
    if(this.cartItem.quantity === 1 && quantity < 0){
      return;
    }

    console.log("Cart item", this.cartItem);
    
    this.cartService.addVideogameToCart(this.cartItem.videogameId, localStorage.getItem("user")!, quantity).subscribe((res) => {
      if(res === "ok"){
        //Should probably be using signals and computed ones for stuff like this
        this.cartItem.quantity += quantity;
        this.onQuantityChange.emit(this.cartItem.quantity);
      }
    });
  }

  removeVideogame(){
    this.onRemove.emit();
  }
}
