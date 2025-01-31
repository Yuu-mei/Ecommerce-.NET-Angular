using HeartbitGamesNet.Models;
using HeartbitGamesNet.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace HeartbitGamesNet.Controllers
{
    //Helper class, will be moved
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [ApiController]
    [Route("admin")]
    public class AdminController
    {
        private readonly VideogameRepository _videogameRepository;
        private readonly AuthRepository _authRepository;
        private readonly StoreRepository _storeRepository;

        public AdminController(VideogameRepository videogameRepository, AuthRepository authRepository, StoreRepository storeRepository)
        {
            _videogameRepository = videogameRepository;
            _authRepository = authRepository;
            _storeRepository = storeRepository;
        }

        [HttpPost("login")]
        public string Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest.Username.IsNullOrEmpty() || loginRequest.Password.IsNullOrEmpty())
            {
                return JsonSerializer.Serialize("error");
            }

            //Check user credentials (should be done in the DB later on)
            if(loginRequest.Username != "" && loginRequest.Password != "")
            {
                return JsonSerializer.Serialize("error");
            }

            var token = GenerateJWTToken(loginRequest.Username);

            return JsonSerializer.Serialize(token);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get_all_videogames_admin")]
        public List<Videogame> GetAllVideogamesAdmin()
        {
            return _videogameRepository.GetAllVideogamesAdmin();
        }

        //[Authorize(Roles = "Admin")]
        //[Consumes("multipart/form-data")]
        //[HttpPost("add_videogame")]
        //public JsonResult AddVideogame([FromForm] Videogame v)
        //{
        //    string res = _videogameRepository.AddVideogame(v);
        //    return new JsonResult(res);
        //}

        //New version to try and handle conversion issues like the OnSale and ReleaseDate fields not being properly converted
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        [HttpPost("add_videogame")]
        public JsonResult AddVideogame(IFormCollection form)
        {
            bool onSale = form["on_sale"] == "true" ? true : false;
            DateTime releaseDate = DateTime.Parse(form["release_date"]!);

            //Creating the videogame object to avoid issues
            Videogame v = new Videogame
            {
                Title = form["title"]!,
                Description = form["description"]!,
                Developer = form["developer"]!,
                Tag = form["tag"]!,
                //Another fix in case the local doesn't work
                Price = double.TryParse(form["price"], out double price) ? price : 0,
                ReleaseDate = releaseDate,
                OnSale = onSale,
                HeaderImg = form.Files["HeaderImg"]!,
                CapsuleImg = form.Files["CapsuleImg"]!
            };

            string res = _videogameRepository.AddVideogame(v);
            return new JsonResult(res);
        }

        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        [HttpPost("register_user")]
        public string RegisterUser(IFormCollection form)
        {
            bool active = form["active"] == "true" ? true : false;

            AppUser user = new AppUser
            {
                Username = form["username"]!,
                Email = form["email"]!,
                Password = form["password"]!,
                Active = active,
                ProfilePic = form.Files["profile_pic"]!
            };

            return JsonSerializer.Serialize(_authRepository.RegisterUser(user));
        }

        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        [HttpPost("edit_videogame")]
        public string EditVideogame(IFormCollection form) 
        {
            bool onSale = form["on_sale"] == "true" ? true : false;
            DateTime releaseDate = DateTime.Parse(form["release_date"]!);

            Videogame v = new Videogame
            {
                Title = form["title"]!,
                Description = form["description"]!,
                Developer = form["developer"]!,
                Tag = form["tag"]!,
                //Another fix in case the local doesn't work
                Price = double.TryParse(form["price"], out double price) ? price : 0,
                ReleaseDate = releaseDate,
                OnSale = onSale,
                HeaderImg = form.Files["HeaderImg"]!,
                CapsuleImg = form.Files["CapsuleImg"]!,
                Id = Convert.ToInt32(form["videogame_id"]!)
            };

            string res = _videogameRepository.EditVideogame(v);

            return JsonSerializer.Serialize(res);
        }

        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        [HttpPost("edit_user")]
        public string EditUser(IFormCollection form)
        {
            bool active = form["active"] == "true" ? true : false;

            AppUser user = new AppUser
            {
                Username = form["username"]!,
                Email = form["email"]!,
                Password = form["password"]!,
                Active = active,
                ProfilePic = form.Files["profile_pic"]!,
                Id = Convert.ToInt32(form["user_id"]!)
            };

            return JsonSerializer.Serialize(_authRepository.EditUser(user));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("delete_videogame")]
        public JsonResult DeleteVideogame(int id)
        {
            string res = _videogameRepository.DeleteVideogameById(id);
            return new JsonResult(res);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("deactivate_videogame")]
        public string DeactivateVideogame(int id)
        {
            string res = _videogameRepository.DeactivateVideogameById(id);
            return JsonSerializer.Serialize(res);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get_all_users")]
        public List<AppUserResp> GetAllUsers()
        {
            List<AppUserResp> res = _authRepository.GetAllUsers();
            return res;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get_user_by_id")]
        public AppUserResp GetUserById(int user_id)
        {
            AppUserResp res = _authRepository.AdminGetUserById(user_id);
            return res;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get_videogame_by_id")]
        public Videogame GetVideogameById(int videogame_id)
        {
            Videogame res = _videogameRepository.GetVideogameDetail(videogame_id);
            return res;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("deactivate_user")]
        public string DeactivateUser(int user_id)
        {
            string res = _authRepository.DeactivateUser(user_id);
            return JsonSerializer.Serialize(res);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get_all_orders")]
        public List<OrderRespAdmin> GetAllOrders()
        {
            return _storeRepository.GetAllOrders();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get_order_detail")]
        public OrderDetailRespAdmin GetOrderDetail(int order_id)
        {
            return _storeRepository.GetOrderDetail(order_id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("delete_order")]
        public string DeleteOrder(int order_id)
        {
            return _storeRepository.DeleteOrder(order_id);
        }

        private string GenerateJWTToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var jwt = new JwtSecurityToken(
                issuer: "",
                audience: "",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
