using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HeartbitGamesNet.Models
{
    public class CartProduct
    {
        [Required]
        public int VideogameId { get; set; }
        [Required]
        [Range(1,100)]
        public int Quantity { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z0-9 :-]{2,}$")]
        public string Title { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z :-]{2,}$")]
        public string Developer { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z :-]{2,}$")]
        public string Tag { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z ,.]{2,}$")]
        public string Description { get; set; }
        [Required]
        [Range(0, 999.99)]
        public Double Price { get; set; }
    }
}
