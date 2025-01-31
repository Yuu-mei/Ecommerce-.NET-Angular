import { Component } from '@angular/core';
import { Videogame } from '../../videogames/types/videogame';
import { FormsModule, NgForm } from '@angular/forms';
import { AdminService } from '../services/admin.service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-add-videogame',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './admin-add-videogame.component.html',
  styleUrl: './admin-add-videogame.component.css'
})
export class AdminAddVideogameComponent {
  videogame:Videogame = {} as Videogame;
  //To avoid issues with the release date as the form passes it as a string but Angular still thinks it's a Date object, let's assign it to a value outside the Videogame class
  release_date:string = "";
  headerImg:File = {} as File;
  capsuleImg:File = {} as File;

  constructor(private adminService:AdminService, private router:Router){}

  onVideogameAdd(addVideogameForm:NgForm){
    const formData = new FormData();
    let on_sale = this.videogame.onSale === true ? "true" : "false";
    console.log("Date",this.release_date);
    console.log("On sale", on_sale);

    formData.append("HeaderImg", this.headerImg);
    formData.append("CapsuleImg", this.capsuleImg);
    formData.append("title", this.videogame.title);
    formData.append("description", this.videogame.description!);
    formData.append("price", this.videogame.price.toFixed(2));
    formData.append("release_date", this.release_date);
    formData.append("tag", this.videogame.tag);
    formData.append("developer", this.videogame.developer);
    formData.append("on_sale", on_sale);

    this.adminService.addVideogame(formData).subscribe(res => {
      if(res == "OK"){
        Swal.fire({
          title: "Game added!",
          icon: "success"
        });
        addVideogameForm.reset();
      }else if (res == "ERROR"){
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Something went wrong creating your game",
        });
      }
    })
  }

  onSaleChange(event:Event){
    const input = event.target as HTMLInputElement;
    this.videogame.onSale = input?.checked ? true : false;
    console.log("Sale changed ", this.videogame.onSale);
  }

  headerImgChange(event:Event){
    const input = event.target as HTMLInputElement;
    this.headerImg = input.files![0];
  }

  capsuleImgChange(event:Event){
    const input = event.target as HTMLInputElement;
    this.capsuleImg = input.files![0];
  }
}
