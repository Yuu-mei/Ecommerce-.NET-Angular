using HeartbitGamesNet.Models;
using HeartbitGamesNet.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HeartbitGamesNet.Controllers
{
    [ApiController]
    [Route("rest")]
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository _authRepository;

        public AuthController(AuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpGet("get_user_data")]
        public Dictionary<string, object> GetProfileInfo(string user_id)
        {
            return _authRepository.GetProfileInfo(user_id);
        }

        [HttpPost("login")]
        public string Login([FromBody] LoginInfo loginInfo)
        {
            return JsonSerializer.Serialize(_authRepository.Login(loginInfo.Email, loginInfo.Password));
        }

        [HttpPost("register_user")]
        [Consumes("multipart/form-data")]
        public string RegisterUser(IFormCollection form)
        {
            AppUser user = new AppUser 
            { 
                Username = form["username"]!,
                Email = form["email"]!,
                Password = form["password"]!,
                ProfilePic = form.Files["profile_pic"]!
            };

            return JsonSerializer.Serialize(_authRepository.RegisterUserAdmin(user));
        }
    }
}
