using HeartbitGamesNet.Models;
using HeartbitGamesNet.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeartbitGamesNet.Controllers
{
    [ApiController]
    [Route("rest")]
    public class VideogameController : ControllerBase
    {
        private readonly VideogameRepository _videogameRepository;

        public VideogameController(VideogameRepository videogameRepository)
        {
            _videogameRepository = videogameRepository;
        }

        [HttpGet("get_all_videogames")]
        public List<Videogame> GetAllVideogames()
        {
            return _videogameRepository.GetAllVideogames();
        }

        [HttpGet("get_videogame_detail")]
        public Videogame GetVideogameDetail(int id)
        {
            return _videogameRepository.GetVideogameDetail(id);
        }

        [HttpGet("get_all_tags")]
        public List<string> GetAllTags()
        {
            return _videogameRepository.GetAllTags();
        }

        [HttpGet("search")]
        public List<Videogame> Search(string developer = "", string tag = "", string title = "")
        {
            return _videogameRepository.Search(developer, tag, title);
        }

        [HttpGet("get_filters")]
        public Dictionary<string, List<string>> GetFilters()
        {
            return _videogameRepository.GetFilters();
        }

        [HttpGet("get_latest_videogames")]
        public List<Videogame> GetLatestVideogames()
        {
            return _videogameRepository.GetLatestVideogames();
        }

        [HttpGet("get_similar_videogames")]
        public List<Videogame> GetSimilarVideogames(string videogame_id, string developer, string tag)
        {
            return _videogameRepository.GetSimilarVideogames(videogame_id, developer, tag);
        }

        [HttpPost("wishlist_videogame")]
        public string WishlistVideogame([FromBody] WishlistPostInfo wishlistPostInfo)
        {
            return _videogameRepository.WishlistVideogame(wishlistPostInfo.VideogameId, wishlistPostInfo.UserId);
        }

        [HttpPost("is_wishlisted")]
        public string IsVideogameWishlisted([FromBody] WishlistPostInfo wishlistPostInfo)
        {
            return _videogameRepository.IsVideogameWishlisted(wishlistPostInfo.VideogameId, wishlistPostInfo.UserId);
        }
    }
}
