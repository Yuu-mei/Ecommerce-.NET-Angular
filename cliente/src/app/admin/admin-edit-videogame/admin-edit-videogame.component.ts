import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Videogame } from '../../videogames/types/videogame';
import { AdminService } from '../services/admin.service';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-admin-edit-videogame',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './admin-edit-videogame.component.html',
  styleUrl: './admin-edit-videogame.component.css'
})
export class AdminEditVideogameComponent implements OnInit{
  videogame:Videogame = {} as Videogame;
  videogame_id:number = -1;
  //To avoid issues with the release date as the form passes it as a string but Angular still thinks it's a Date object, let's assign it to a value outside the Videogame class
  release_date:string = "";
  headerImg:File = {} as File;
  capsuleImg:File = {} as File;
  
  constructor(private adminService:AdminService, private activatedRoute:ActivatedRoute, private router:Router) {}

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.videogame_id = Number(params.get("id"));
      this.adminService.getVideogameById(this.videogame_id).subscribe(res => {
        this.videogame = res;
      });
    })
  }

  headerImgChange(event:Event){
    const input = event.target as HTMLInputElement;
    this.headerImg = input.files![0];
  }

  capsuleImgChange(event:Event){
    const input = event.target as HTMLInputElement;
    this.capsuleImg = input.files![0];
  }

  onSaleChange(event:Event){
    const input = event.target as HTMLInputElement;
    this.videogame.onSale = input?.checked ? true : false;
    console.log("Sale changed ", this.videogame.onSale);
  }

  onVideogameEdit(editVideogameForm:NgForm){
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
    formData.append("videogame_id", this.videogame_id.toString())

    this.adminService.editVideogame(formData).subscribe(res => {
      if(res == "OK"){
        Swal.fire({
          title: "Game edited!",
          icon: "success"
        });
        this.router.navigate(["admin/videogame-list"]);
      }else{
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Something went wrong editing your game",
        });
      }
    })
  }
}
