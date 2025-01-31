using System.ComponentModel.DataAnnotations;

namespace HeartbitGamesNet.Models
{
    public class LoginInfo
    {
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\.-]+)@([0-9a-zA-Z\.-]+)\.([a-zA-Z\.]+)$")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{8,20}$")]
        public string Password { get; set; }
    }
}
