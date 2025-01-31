using HeartbitGamesNet.Constants;
using HeartbitGamesNet.Models;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace HeartbitGamesNet.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public string RegisterUser(AppUser user)
        {
            //If user exists, return
            if (CheckUserExists(user.Email)) return "error";

            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new(SQLConstants.SQL_ADD_USER, conn);

            //Parameters
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);

            cmd.ExecuteNonQuery();

            //Generated ID
            long gId = cmd.LastInsertedId;

            //Upload imgs
            FormFile profilePic = (FormFile) user.ProfilePic;

            string profilePicPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "users");

            string profilePicFilename = Path.Combine(profilePicPath, gId.ToString() + ".jpg");

            using (FileStream stream = new(profilePicFilename, FileMode.Create))
            {
                profilePic.CopyTo(stream);
            }

            conn.Close();

            return "ok";
        }

        //Since it has the active field on the admin panel, we have to redo the code
        public string RegisterUserAdmin(AppUser user)
        {
            //If user exists, return
            if (CheckUserExists(user.Email)) return "error";

            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new(SQLConstants.SQL_ADD_USER, conn);

            //Parameters
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Active", user.Active);

            cmd.ExecuteNonQuery();

            //Generated ID
            long gId = cmd.LastInsertedId;

            //Upload imgs
            FormFile profilePic = (FormFile)user.ProfilePic;

            string profilePicPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "users");

            string profilePicFilename = Path.Combine(profilePicPath, gId.ToString() + ".jpg");

            using (FileStream stream = new(profilePicFilename, FileMode.Create))
            {
                profilePic.CopyTo(stream);
            }

            conn.Close();

            return "ok";
        }

        public bool CheckUserExists(string email)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new(SQLConstants.SQL_CHECK_USER_EXISTS, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            //ExecuteScalar returns only a single row, better than using the reader in this case
            object result = cmd.ExecuteScalar();
            if (result != null) 
            {
                return Convert.ToInt32(result.ToString()) > 0;
            }

            conn.Close();

            return false;
        }

        public bool CheckUserActive(string email)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new(SQLConstants.SQL_CHECK_USER_ACTIVE, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                if (result.ToString() == "0") return false;
                return true;
            }

            conn.Close();

            return false;
        }

        public string Login(string email, string password)
        {
            //Regex patterns
            string emailPat = @"^([a-zA-Z0-9_\.-]+)@([0-9a-zA-Z\.-]+)\.([a-zA-Z\.]+)$";
            string pwdPat = @"^[a-zA-Z0-9]{8,20}$";

            //Regex match
            Regex emailRegex = new Regex(emailPat, RegexOptions.IgnoreCase);
            Regex pwdRegex = new Regex(pwdPat, RegexOptions.IgnoreCase);

            Match emailMatch = emailRegex.Match(email);
            Match pwdMatch = pwdRegex.Match(password);

            if (!emailMatch.Success) 
            {
                return "Invalid email";
            }
            if (!pwdMatch.Success)
            {
                return "Invalid password";
            }

            if (!CheckUserActive(email)) {
                return "Inactive User";
            }

            //Check email and password matches
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new(SQLConstants.SQL_CHECK_USER_PWD, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);

            object result = cmd.ExecuteScalar();

            if (result != null)
            {
                //Returns the ID of the user
                return result.ToString()!;
            }
            else
            {
                return "error";
            }

            conn.Close();
        }

        public Dictionary<string, object> GetProfileInfo(string user_id)
        {
            UserInfo userInfo = GetUserData(user_id);
            List<OrderInfo> ordersInfo = GetOrdersInfo(user_id);
            List<WishlistVideogameInfo> wishlistedVideogameInfos = GetWishlistedVideogamesInfo(user_id);

            Dictionary<string, object> res = new Dictionary<string, object>();

            res.Add("orders", ordersInfo);
            res.Add("user_info", userInfo);
            res.Add("wishlist_info", wishlistedVideogameInfos);

            return res;
        }

        public UserInfo GetUserData(string user_id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(SQLConstants.SQL_OBTAIN_USER_DATA, conn);
            cmd.Parameters.AddWithValue("@Id", user_id);

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new UserInfo
                {
                    Username = reader["username"].ToString()!,
                    Email = reader["email"].ToString()!
                };
            }

            reader.Close();
            conn.Close();

            return null;
        }
        
        

        public List<OrderInfo> GetOrdersInfo(string user_id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(SQLConstants.SQL_OBTAIN_USER_ORDER, conn);
            cmd.Parameters.AddWithValue("@Id", user_id);

            MySqlDataReader reader = cmd.ExecuteReader();
            List<OrderInfo> orderInfos = new List<OrderInfo>();

            while (reader.Read())
            {
                orderInfos.Add(new OrderInfo
                {
                    OrderId = Convert.ToInt32(reader["id"].ToString()),
                    VideogameId = Convert.ToInt32(reader["videogame_id"].ToString()),
                    Title = reader["title"].ToString()!,
                    Quantity = reader["quantity"].ToString()!
                });
            }

            reader.Close();
            conn.Close();

            return orderInfos;
        }

        public List<WishlistVideogameInfo> GetWishlistedVideogamesInfo(string user_id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(SQLConstants.SQL_OBTAIN_USER_WISHLIST, conn);
            cmd.Parameters.AddWithValue("@Id", user_id);

            MySqlDataReader reader = cmd.ExecuteReader();
            List<WishlistVideogameInfo> wishlistInfo = new List<WishlistVideogameInfo>();

            while (reader.Read())
            {
                wishlistInfo.Add(new WishlistVideogameInfo
                {
                    Id = Convert.ToInt32(reader["id"].ToString()),
                    Title = reader["title"].ToString()!,
                });
            }

            reader.Close();
            conn.Close();

            return wishlistInfo;
        }

        public List<AppUserResp> GetAllUsers()
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(SQLConstants.SQL_OBTAIN_ALL_USERS, conn);

            MySqlDataReader reader = cmd.ExecuteReader();
            List<AppUserResp> appUsers = new List<AppUserResp>();

            while (reader.Read())
            {
                appUsers.Add(new AppUserResp
                {
                    Username = reader["username"].ToString()!,
                    Email = reader["email"].ToString()!,
                    Id = Convert.ToInt32(reader["id"].ToString()),
                    Active = Convert.ToInt32(reader["active"].ToString())
                });
            }

            reader.Close();
            conn.Close();

            return appUsers;
        }

        public AppUserResp AdminGetUserById(int user_id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(SQLConstants.SQL_OBTAIN_USER_ADMIN, conn);
            cmd.Parameters.AddWithValue("@UserId", user_id);

            MySqlDataReader reader = cmd.ExecuteReader();
            AppUserResp appUser = null;

            if (reader.Read())
            {
                appUser = new AppUserResp
                {
                    Id = Convert.ToInt32(reader["id"].ToString()),
                    Username = reader["username"].ToString()!,
                    Email = reader["email"].ToString()!,
                    Password = reader["password"].ToString()!,
                    Active = Convert.ToInt32(reader["active"].ToString()),
                };
            }

            reader.Close();
            conn.Close();

            return appUser;
        }
        
        public string EditUser(AppUser user)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new(SQLConstants.SQL_UPDATE_USER, conn);

            //Parameters
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Active", user.Active);
            cmd.Parameters.AddWithValue("@UserId", user.Id);

            cmd.ExecuteNonQuery();

            //Upload imgs
            FormFile profilePic = (FormFile)user.ProfilePic;

            string profilePicPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "users");

            string profilePicFilename = Path.Combine(profilePicPath, user.Id.ToString() + ".jpg");

            using (FileStream stream = new(profilePicFilename, FileMode.Create))
            {
                profilePic.CopyTo(stream);
            }

            conn.Close();

            return "OK";
        }

        public string DeactivateUser(int user_id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(SQLConstants.SQL_DEACTIVATE_USER, conn);
            cmd.Parameters.AddWithValue("@UserId", user_id);
            cmd.ExecuteNonQuery();

            conn.Close();

            return "ok";
        }


    }
}
