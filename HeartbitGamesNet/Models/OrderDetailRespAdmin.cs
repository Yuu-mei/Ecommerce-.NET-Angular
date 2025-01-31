namespace HeartbitGamesNet.Models
{
    public class OrderDetailRespAdmin
    {
        public string VideogameIds { get; set; }
        public int Quantity { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string CardNumber { get; set; }
        public int CCV { get; set; }
        public string CardOwner { get; set; }
        //In order to send only one item in the response, we will hold the data of the videogames inside this order detail response object
        public List<OrderVideogame> OrderVideogames { get; set; }
    }
}
