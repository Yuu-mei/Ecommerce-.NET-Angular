import { AfterViewChecked, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { VideogameService } from '../../services/videogame.service';
import { Videogame } from '../types/videogame';
import { ActivatedRoute, Router } from '@angular/router';
import { VideogameCardComponent } from "../videogame-card/videogame-card.component";
import { CartService } from '../../cart/services/cart.service';
import { AuthService } from '../../auth/services/auth.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-videogame-detail',
  standalone: true,
  imports: [
    VideogameCardComponent
  ],
  templateUrl: './videogame-detail.component.html',
  styleUrl: './videogame-detail.component.css'
})
export class VideogameDetailComponent implements OnInit, AfterViewChecked{
  videogame_id:number = -1;
  loading:boolean = true;
  error:boolean = false;
  loadingSimilar:boolean = true;
  errorSimilar:boolean = false;
  videogame:Videogame = {
    id: -1,
    developer: "none",
    price: 0,
    release_date: new Date("1999-01-01"),
    tag: "none",
    title: "none",
    description: "none",
    onSale: false
  }
  similarVideogames:Videogame[] = [];
  isUserLoggedIn:boolean = false;
  showTip:boolean = false;

  //Wishlist
  wishlisted:boolean = false;
  wishlist_flag:boolean = false;
  @ViewChild('wishlistIcon') wishlistIcon!:ElementRef;
  count:number = 0;

  constructor(private videogameService:VideogameService, private activatedRoute:ActivatedRoute, private cartService:CartService, private authService:AuthService, 
    private router:Router) {}

  ngOnInit():void{   
    // Reactive way to get the videogame_id
    this.activatedRoute.paramMap.subscribe({
      next: (params) => {
        this.videogame_id = Number(params.get("id"));
        //Undo the wishlist flag if we change route
        this.wishlist_flag = false;
        this.obtainVideogameDetails();
      },
      error: (err) => {
        console.error(`Error loading current videogame with id: ${this.videogame_id}`, err);
        this.error = true;
      }
    });
    // Subscribe to changes on the local storage to avoid adding products to a cart if you are not logged in
    this.authService.isLoggedIn$.subscribe((logged) => {
      this.isUserLoggedIn = logged;
    });  
  }

  ngAfterViewChecked(): void {
    if(!this.isUserLoggedIn) return;
    if(this.count >= 2) return;
    this.count++;
    if(this.wishlistIcon != undefined){
      if(this.wishlist_flag) return;
      this.videogameService.isVideogameWishlisted(this.videogame_id, Number(localStorage.getItem("user"))).subscribe((res) => {
        if(res === "yes"){
          this.wishlistIcon.nativeElement.classList.add("clicked");
          this.wishlist_flag = true;
        }else{
          this.wishlistIcon.nativeElement.classList.remove("clicked");
          this.wishlist_flag = true;
        }
      });
    }
  }

  obtainVideogameDetails(){
    this.videogameService.obtainVideogameDetails(this.videogame_id).subscribe({
      next: (videogame:Videogame) => {
        this.videogame = videogame;
        console.log(this.videogame);
        this.loading = false;
      },
      error: (err) => {
        console.error(`Error loading videogame: ${err}`);
        this.error = true;
        this.loading = false;
      },
      complete: () => {
        this.loading = false;

        // I should probably take this out in another function for a more clear view
        this.videogameService.obtainSimilarVideogames(this.videogame_id, this.videogame.developer, this.videogame.tag).subscribe({
          next: (videogames:Videogame[]) => {
            this.similarVideogames = videogames;
            this.loadingSimilar = false;
          },
          error: (err) => {
            console.error(`Error loading similar videogames: ${err}`);
            this.loadingSimilar = false;
            this.errorSimilar = true;
          },
          complete: () => {
            this.loadingSimilar = false;
          }
        });
      }
    });
  }

  addVideogameToCart(){
    this.cartService.addVideogameToCart(this.videogame_id, localStorage.getItem("user")!, 1).subscribe(res => {
      if(res === "ok"){
        Swal.fire({
          title: "Product added to the cart!",
          icon: "success"
        });
      }else{
        console.error("Something went wrong adding the product to the cart");
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Something went wrong adding the product to the cart",
        });
      }
    });
  }

  getVideogamesByDeveloper(developer:string){
    this.router.navigate(['search'], {
      queryParams: {
        developer
      }
    });
  }

  getVideogamesByTag(tag:string){
    this.router.navigate(['search'], {
      queryParams: {
        tag
      }
    });
  }

  onButtonHover(){
    if(this.isUserLoggedIn){
      this.showTip = false;
    }else {
      this.showTip = true;
    }
  }

  wishlist(videogame_id:number, wishlistIcon:HTMLElement){
    // Same as Twitter, we first do the action so the user believes it's working even if there has been some sort of issue in the way there
    if(wishlistIcon.classList.contains("clicked")){
      wishlistIcon.classList.remove("clicked");
    }else{
      wishlistIcon.classList.add("clicked");
    }

    this.videogameService.wishlistVideogame(videogame_id, Number(localStorage.getItem("user"))).subscribe((res) => {
      if(res === "undo-wishlist"){
        this.wishlisted = false;
      }else{
        this.wishlisted = true;
      }
    });
  }
}
