using HeartbitGamesNet.Models;

namespace HeartbitGamesNet.Repositories
{
    public interface IStoreRepository
    {
        public string AddCartProduct(string videogame_id, string user_id, int quantity);
        public List<CartProduct> GetCartProducts(string user_id);
        public string RegisterOrder(Order order);
        public string RemoveCartItem(string videogame_id, string user_id);
        public List<OrderRespAdmin> GetAllOrders();
        public OrderDetailRespAdmin GetOrderDetail(int order_id);
        public string DeleteOrder(int order_id);
    }
}
