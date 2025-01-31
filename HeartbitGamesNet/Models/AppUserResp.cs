using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HeartbitGamesNet.Models
{
    public class AppUserResp
    {
        private int _id;
        public int Id { get { return _id; } set { _id = value; } }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Active { get; set; }
    }
}
