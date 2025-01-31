using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HeartbitGamesNet.Models
{
    public class RemoveCartItemPost
    {
        [Required]
        [JsonPropertyName("videogame_id")]
        public string VideogameId { get; set; }
        [Required]
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }
}
