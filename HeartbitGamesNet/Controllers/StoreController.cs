using HeartbitGamesNet.Models;
using HeartbitGamesNet.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HeartbitGamesNet.Controllers
{
    [ApiController]
    [Route("rest")]
    public class StoreController : ControllerBase
    {
        private readonly StoreRepository _storeRepository;

        public StoreController(StoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        [HttpPost("add_cart_product")]
        public string AddCartProduct([FromBody] CartProductPost cartProductPost)
        {
            return _storeRepository.AddCartProduct(cartProductPost.VideogameId, cartProductPost.UserId, cartProductPost.Quantity);
        }

        [HttpGet("get_cart_products")]
        public List<CartProduct> GetCartProducts(string user_id)
        {
            return _storeRepository.GetCartProducts(user_id);
        }

        [HttpPost("register_order")]
        public string RegisterOrder(Order order)
        {
            return _storeRepository.RegisterOrder(order);
        }

        [HttpPost("remove_cart_item")]
        public string RemoveCartItem([FromBody] RemoveCartItemPost removeCartItemPost)
        {
            return _storeRepository.RemoveCartItem(removeCartItemPost.VideogameId, removeCartItemPost.UserId);
        }
    }
}
