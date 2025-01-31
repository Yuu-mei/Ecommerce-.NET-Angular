using HeartbitGamesNet.Constants;
using HeartbitGamesNet.Models;
using MySql.Data.MySqlClient;
using System.Text.Json;

namespace HeartbitGamesNet.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly string _connectionString;

        public StoreRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        //Helper method that will check if a cart exists (if it doesn't it creates one) and return the id of the cart 
        private int CheckCart(MySqlConnection conn, string user_id)
        {
            MySqlCommand cmd = new(SQLConstants.SQL_CHECK_CART, conn);
            cmd.Parameters.AddWithValue("@UserId", user_id);

            object res = cmd.ExecuteScalar();
            int cart_id = -1;
            if (res != null)
            {
                cart_id = Convert.ToInt32(res);
            }
            else
            {
                //Create cart in case it isn´t active/doesn´t exist
                MySqlCommand cartCmd = new(SQLConstants.SQL_CREATE_CART, conn);
                cartCmd.Parameters.AddWithValue("@UserId", user_id);

                cartCmd.ExecuteNonQuery();

                cart_id = Convert.ToInt32(cartCmd.LastInsertedId);
            }

            return cart_id;
        }

        public string AddCartProduct(string videogame_id, string user_id, int quantity)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            //Check if there is an active cart
            int cart_id = CheckCart(conn, user_id);

            //Check if product is already on cart
            MySqlCommand checkProductCmd = new(SQLConstants.SQL_CHECK_PRODUCT_IN_CART, conn);
            checkProductCmd.Parameters.AddWithValue("@CartId", cart_id);
            checkProductCmd.Parameters.AddWithValue("@VideogameId", videogame_id);
            MySqlDataReader reader = checkProductCmd.ExecuteReader();
            bool productAlreadyExists = false;
            int cart_product_id = -1;
            int updated_quantity = -1;

            //Update quantity
            while (reader.Read()) 
            {
                productAlreadyExists = true;
                cart_product_id = Convert.ToInt32(reader["cart_product_id"].ToString());
                updated_quantity = Convert.ToInt32(reader["quantity"].ToString())+quantity;
            }
            //You can't do two operations if a reader is open
            reader.Close();

            if (productAlreadyExists)
            {
                MySqlCommand updateProductCmd = new(SQLConstants.SQL_UPDATE_CART_PRODUCT, conn);
                updateProductCmd.Parameters.AddWithValue("@CartProductId", cart_product_id);
                updateProductCmd.Parameters.AddWithValue("@Quantity", updated_quantity);
                updateProductCmd.ExecuteNonQuery();
            }
            else
            {
                MySqlCommand insertProductCmd = new(SQLConstants.SQL_INSERT_CART_PRODUCT, conn);
                insertProductCmd.Parameters.AddWithValue("@CartId", cart_id);
                insertProductCmd.Parameters.AddWithValue("@VideogameId", videogame_id);
                insertProductCmd.Parameters.AddWithValue("@Quantity", quantity);
                insertProductCmd.ExecuteNonQuery();
            }

            conn.Close();

            return JsonSerializer.Serialize("ok");
        }

        public List<CartProduct> GetCartProducts(string user_id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            //We check if there is a cart in DB and retrieve an id
            int cart_id = CheckCart(conn, user_id);
            //Retrieve data from the cart
            MySqlCommand cartProductsCmd = new(SQLConstants.SQL_GET_CART_PRODUCTS, conn);
            cartProductsCmd.Parameters.AddWithValue("@CartId", cart_id);
            List<CartProduct> cartProducts = new List<CartProduct>();

            MySqlDataReader reader = cartProductsCmd.ExecuteReader();

            while (reader.Read())
            {
                cartProducts.Add(new CartProduct
                {
                    VideogameId = Convert.ToInt32(reader["videogame_id"].ToString()),
                    Quantity = Convert.ToInt32(reader["quantity"].ToString()),
                    Title = reader["title"].ToString()!,
                    Developer = reader["developer"].ToString()!,
                    Tag = reader["tag"].ToString()!,
                    Description = reader["description"].ToString()!,
                    Price = Convert.ToDouble(reader["price"].ToString()),
                });
            }

            reader.Close();
            conn.Close();

            return cartProducts;
        }

        public string RegisterOrder(Order order)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            //Create order in DB
            MySqlCommand orderCmd = new(SQLConstants.SQL_CREATE_ORDER, conn);
            orderCmd.Parameters.AddWithValue("@FullName", order.FullName);
            orderCmd.Parameters.AddWithValue("@Address", order.Address);
            orderCmd.Parameters.AddWithValue("@Country", order.Country);
            orderCmd.Parameters.AddWithValue("@State", order.State);
            orderCmd.Parameters.AddWithValue("@ZipCode", order.ZipCode);
            orderCmd.Parameters.AddWithValue("@CardNumber", order.CardNumber);
            orderCmd.Parameters.AddWithValue("@CCV", order.CCV);
            orderCmd.Parameters.AddWithValue("@CardOwner", order.CardOwner);
            orderCmd.Parameters.AddWithValue("@UserId", order.UserId);
            orderCmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            orderCmd.ExecuteNonQuery();
            //Saving the order id for the videogame_order table
            long order_id = orderCmd.LastInsertedId;
            //Getting cart id of the user to update its status and get the products from the cart
            MySqlCommand cartCmd = new(SQLConstants.SQL_GET_CART, conn);
            cartCmd.Parameters.AddWithValue("@UserId", order.UserId);
            int cart_id = Convert.ToInt32(cartCmd.ExecuteScalar());
            //Getting products
            MySqlCommand cartProductCmd = new(SQLConstants.SQL_GET_CART_PRODUCTS_FROM_DB, conn);
            cartProductCmd.Parameters.AddWithValue("@CartId", cart_id);
            MySqlDataReader reader = cartProductCmd.ExecuteReader();
            List<ReducedCartProduct> cartProducts = new List<ReducedCartProduct>();

            while (reader.Read())
            {
                cartProducts.Add(new ReducedCartProduct
                {
                    VideogameId = Convert.ToInt32(reader["videogame_id"].ToString()),
                    Quantity = Convert.ToInt32(reader["quantity"].ToString()),
                });
            }

            reader.Close();

            //Inserting products to the videogame order table
            foreach (ReducedCartProduct product in cartProducts)
            {
                MySqlCommand videogameOrderCmd = new(SQLConstants.SQL_CREATE_VIDEOGAME_ORDER, conn);
                videogameOrderCmd.Parameters.AddWithValue("@OrderId", order_id);
                videogameOrderCmd.Parameters.AddWithValue("@VideogameId", product.VideogameId);
                videogameOrderCmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                videogameOrderCmd.ExecuteNonQuery();
            }

            //Clearing the cart
            MySqlCommand clearCartCmd = new(SQLConstants.SQL_CLEAR_CART, conn);
            clearCartCmd.Parameters.AddWithValue("@UserId", order.UserId);
            clearCartCmd.Parameters.AddWithValue("@@CartId", cart_id);
            clearCartCmd.ExecuteNonQuery();

            conn.Close();

            return JsonSerializer.Serialize("ok");
        }

        public string RemoveCartItem(string videogame_id, string user_id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            //Check if there is a cart in DB and retrieve an id
            int cart_id = CheckCart(conn, user_id);
            //Check if the product exists in the cart
            MySqlCommand checkProductCmd = new MySqlCommand(SQLConstants.SQL_CHECK_PRODUCT_IN_CART, conn);
            checkProductCmd.Parameters.AddWithValue("@CartId", cart_id);
            checkProductCmd.Parameters.AddWithValue("@VideogameId", videogame_id);
            MySqlDataReader reader = checkProductCmd.ExecuteReader();

            if (reader.Read())
            {
                reader.Close();
                //Remove product from cart
                MySqlCommand removeProductCmd = new MySqlCommand(SQLConstants.SQL_REMOVE_CART_PRODUCT, conn);
                removeProductCmd.Parameters.AddWithValue("@VideogameId", videogame_id);
                removeProductCmd.Parameters.AddWithValue("@CartId", cart_id);
                removeProductCmd.ExecuteNonQuery();
                conn.Close();
                return JsonSerializer.Serialize("ok");
            }
            else
            {
                reader.Close();
                conn.Close();
                return JsonSerializer.Serialize("error");
            }
        }

        public string DeleteOrder(int order_id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand orderCmd = new MySqlCommand(SQLConstants.SQL_DELETE_ORDER, conn);
            orderCmd.Parameters.AddWithValue("@Id", order_id);
            orderCmd.ExecuteNonQuery();
            MySqlCommand videogameOrderCmd = new MySqlCommand(SQLConstants.SQL_DELETE_VIDEOGAMEORDER, conn);
            videogameOrderCmd.Parameters.AddWithValue("@Id", order_id);
            videogameOrderCmd.ExecuteNonQuery();

            conn.Close();

            return "ok";
        }

        public OrderDetailRespAdmin GetOrderDetail(int order_id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(SQLConstants.SQL_OBTAIN_ORDER_DETAILS_ADMIN, conn);
            //Parameters
            cmd.Parameters.AddWithValue("@OrderId", order_id);

            MySqlDataReader reader = cmd.ExecuteReader();
            OrderDetailRespAdmin orderDetail = null;

            if (reader.Read())
            {
                orderDetail = new OrderDetailRespAdmin
                {
                    Address = reader["address"].ToString()!,
                    CardNumber = reader["card_number"].ToString()!,
                    CardOwner = reader["card_owner"].ToString()!,
                    CCV = Convert.ToInt32(reader["ccv"].ToString()),
                    Country = reader["country"].ToString()!,
                    State = reader["state"].ToString()!,
                    ZipCode = reader["zip_code"].ToString()!,
                    FullName = reader["full_name"].ToString()!,
                    Quantity = Convert.ToInt32(reader["quantity"].ToString()),
                    VideogameIds = reader["videogame_ids"].ToString()!
                };
            }
            else
            {
                //Just to avoid issues when splitting the ids
                return orderDetail;
            }

            reader.Close();

            //Now we retrieve the data for the videogames
            //Yippie we have to dynamically create the values for the SQL IN clause
            string[] videogameIds = orderDetail.VideogameIds.Split(",");
            List<string> sqlPlaceholders = new List<string>();
            for (int i = 0; i < videogameIds.Length; i++)
            {
                sqlPlaceholders.Add($"@Id{i}");
            }

            string videogamesSql = $@"SELECT v.title, v.price, vo.quantity, v.id FROM videogames AS v JOIN videogameorder AS vo ON vo.videogame_id = v.id AND vo.order_id = @OrderId WHERE v.id IN ({string.Join(", ", sqlPlaceholders)});";
            MySqlCommand videogamesCmd = new MySqlCommand(videogamesSql, conn);
            videogamesCmd.Parameters.AddWithValue("@OrderId", order_id);
            for (int i = 0; i < videogameIds.Length; i++)
            {
                videogamesCmd.Parameters.AddWithValue($"@Id{i}", videogameIds[i]);
            }

            MySqlDataReader videogameReader = videogamesCmd.ExecuteReader();
            List<OrderVideogame> orderVideogames = new List<OrderVideogame>();

            while (videogameReader.Read())
            {
                orderVideogames.Add(new OrderVideogame
                {
                    Id = Convert.ToInt32(videogameReader["id"].ToString()),
                    Title = videogameReader["title"].ToString()!,
                    Quantity = Convert.ToInt32(videogameReader["quantity"].ToString()),
                    Price = Convert.ToDouble(videogameReader["price"].ToString())
                });
            }

            videogameReader.Close();

            //Add it to the response
            orderDetail.OrderVideogames = orderVideogames;

            return orderDetail;
        }

        public List<OrderRespAdmin> GetAllOrders()
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(SQLConstants.SQL_OBTAIN_ALL_ORDERS_ADMIN, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<OrderRespAdmin> orders = new List<OrderRespAdmin>();

            while (reader.Read())
            {
                orders.Add(new OrderRespAdmin
                {
                    OrderId = Convert.ToInt32(reader["id"].ToString()),
                    FullName = reader["full_name"].ToString()!,
                    Address = reader["address"].ToString()!,
                    Email = reader["email"].ToString()!
                });
            }

            reader.Close();
            conn.Close();

            return orders;
        }
    }
}
