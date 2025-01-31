using HeartbitGamesNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace HeartbitGamesNet.Repositories
{
    public interface IVideogameRepository
    {
        public List<Videogame> GetAllVideogames();
        public List<Videogame> GetLatestVideogames();
        public List<Videogame> GetSimilarVideogames(string videogame_id, string developer, string tag);
        public string AddVideogame(Videogame v);
        public string EditVideogame(Videogame v);
        public List<Videogame> GetAllVideogamesAdmin();
        public List<string> GetAllTags();
        public List<string> GetAllDevs();
        public List<Videogame> Search(string dev="", string tag="", string title="");
        public Dictionary<string, List<string>> GetFilters();
        public Videogame GetVideogameDetail(int id);
        public string DeleteVideogameById(int id);
        public string DeactivateVideogameById(int id);
        public string WishlistVideogame(string videogame_id, string user_id);
        public string IsVideogameWishlisted(string videogame_id, string user_id);
    }
}
