export type OrderDetailRespAdmin = {
    videogameIds?:number,
    quantity?:number,
    fullName?:string,
    address?:string,
    country?:string,
    state?:string,
    zipCode?:string,
    cardOwner?:string,
    cardNumber?:string,
    ccv?:number,
    orderVideogames?:OrderDetailVideogameResp[]
}

export type OrderDetailVideogameResp = {
    id?:number,
    title?:string,
    price?:number,
    quantity?:number
}