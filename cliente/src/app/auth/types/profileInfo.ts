import { Videogame } from "../../videogames/types/videogame";

export interface ProfileInfo {
    orders:        Order[];
    user_info:     UserInfo;
    wishlist_info: Videogame[];
}

export interface Order {
    orderId:        number;
    title:          string;
    quantity:       string;
    videogameId:    number;
}

export interface UserInfo {
    username: string;
    email:    string;
}