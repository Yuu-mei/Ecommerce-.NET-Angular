using System.ComponentModel.DataAnnotations;

namespace HeartbitGamesNet.Models
{
    public class ReducedCartProduct
    {
        [Required]
        public int VideogameId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
