using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HeartbitGamesNet.Models
{
    public class Videogame
    {
        private int _id;
        public int Id { get { return _id; } set { _id = value; } }
        [Required]
        [RegularExpression(@"^[A-Za-z0-9 :-]{2,}$")]
        public string Title { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z ,.]{2,}$")]
        public string Description { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z :-]{2,}$")]
        public string Developer { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z :-]{2,}$")]
        public string Tag { get; set; }
        [Required]
        [Range(0, 999.99)]
        public Double Price { get; set; }
        [Required]
        public bool OnSale { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        [JsonPropertyName("headerImage")]
        public IFormFile HeaderImg { get; set; }
        [Required]
        [JsonPropertyName("capsuleImage")]
        public IFormFile CapsuleImg { get; set; }
    }
}
