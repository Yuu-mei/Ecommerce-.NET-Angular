using HeartbitGamesNet.Models;

namespace HeartbitGamesNet.Repositories
{
    public interface IAuthRepository
    {
        public List<AppUserResp> GetAllUsers();
        public List<OrderInfo> GetOrdersInfo(string user_id);
        public string RegisterUser(AppUser user);
        public string EditUser(AppUser user);
        public string DeactivateUser(int user_id);
        public string Login(string email, string password);
        public Dictionary<string, object> GetProfileInfo(string user_id);
        public UserInfo GetUserData(string user_id);
        public AppUserResp AdminGetUserById(int user_id);
        public List<WishlistVideogameInfo> GetWishlistedVideogamesInfo(string user_id);
        public bool CheckUserExists(string email);
        public bool CheckUserActive(string email);
    }
}
