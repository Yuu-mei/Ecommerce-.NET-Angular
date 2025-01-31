using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HeartbitGamesNet.Models
{
    public class AppUser
    {
        private int _id;
        public int Id { get { return _id; } set { _id = value; } }
        [Required]
        [RegularExpression(@"^[a-zA-Z_ -]{3,30}$")]
        public string Username { get; set; }
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\.-]+)@([0-9a-zA-Z\.-]+)\.([a-zA-Z\.]+)$")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{8,20}$")]
        public string Password { get; set; }
        [Required]
        [JsonPropertyName("profile_pic")]
        public IFormFile ProfilePic { get; set; }
        public bool Active { get; set; }
    }
}
