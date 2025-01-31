namespace HeartbitGamesNet.Constants
{
    public class SQLConstants
    {
        public const string SQL_OBTAIN_ALL_VIDEOGAMES = "SELECT * FROM videogames WHERE on_sale = 1;";
        public const string SQL_OBTAIN_LATEST_VIDEOGAMES = "SELECT * FROM videogames AS v WHERE on_sale = 1 ORDER BY v.id DESC LIMIT 3;";
        public const string SQL_OBTAIN_SIMILAR_VIDEOGAMES = "SELECT * FROM videogames WHERE (LOWER(tag) LIKE LOWER(@Tag) OR LOWER(developer) LIKE LOWER(@Developer)) AND id != @Id";
        public const string SQL_OBTAIN_ALL_TAGS = "SELECT DISTINCT(tag) FROM videogames;";
        public const string SQL_OBTAIN_ALL_DEVS = "SELECT DISTINCT(developer) FROM videogames;";
        public const string SQL_OBTAIN_ALL_VIDEOGAMES_ADMIN = "SELECT * FROM videogames;";
        public const string SQL_OBTAIN_ALL_ORDERS_ADMIN = "SELECT o.id, o.full_name, o.address, u.email FROM orders AS o JOIN appusers AS u ON u.id = o.user_id;";
        public const string SQL_OBTAIN_ORDER_DETAILS_ADMIN = "SELECT GROUP_CONCAT(vo.videogame_id) AS videogame_ids, vo.quantity, od.full_name, od.country, od.state, od.zip_code, od.address, od.card_number, od.ccv, od.card_owner FROM orders AS od JOIN appusers AS u ON od.user_id = u.id JOIN videogameorder AS vo ON vo.order_id = od.id  WHERE od.id = @OrderId";
        public const string SQL_OBTAIN_ORDER_VIDEOGAME_DETAILS_ADMIN = "SELECT v.title, v.price, vo.quantity, v.id FROM videogames AS v JOIN videogameorder AS vo ON vo.videogame_id = v.id AND vo.order_id = @OrderId WHERE v.id IN (@VideogameIds);";
        public const string SQL_OBTAIN_USER_DATA = "SELECT u.username, u.email FROM appusers AS u WHERE u.id = @Id";
        public const string SQL_OBTAIN_USER_ADMIN = "SELECT * FROM appusers WHERE id = @UserId";
        public const string SQL_OBTAIN_USER_ORDER = "SELECT o.id, v.title, vo.quantity, v.id AS videogame_id FROM orders AS o JOIN videogameorder AS vo ON vo.order_id = o.id JOIN videogames AS v ON vo.videogame_id = v.id WHERE o.user_id = @Id";
        public const string SQL_OBTAIN_USER_WISHLIST = "SELECT v.title, v.id FROM wishlist AS w JOIN videogames AS v ON w.videogame_id = v.id WHERE w.user_id = @Id";
        public const string SQL_OBTAIN_ALL_USERS = "SELECT id, username, email, active FROM appusers;";
        public const string SQL_BASE_SEARCH = "SELECT * FROM videogames WHERE 1=1";
        public const string SQL_OBTAIN_VIDEOGAME_DETAIL = "SELECT * FROM videogames WHERE id = @Id";
        public const string SQL_DELETE_VIDEOGAME = "DELETE FROM videogames WHERE id = @Id";
        public const string SQL_DELETE_ORDER = "DELETE FROM orders WHERE id = @Id";
        public const string SQL_DELETE_VIDEOGAMEORDER = "DELETE FROM videogameorder WHERE order_id = @Id";
        public const string SQL_DEACTIVATE_VIDEOGAME = "UPDATE videogames SET on_sale = 0 WHERE id = @VideogameId";
        public const string SQL_DEACTIVATE_USER = "UPDATE appusers SET active = 0 WHERE id = @UserId";
        public const string SQL_CHECK_VIDEOGAME_WISHLISTED = "SELECT COUNT(*) FROM wishlist WHERE user_id = @UserId AND videogame_id = @VideogameId";
        public const string SQL_ADD_WISHLIST_VIDEOGAME = "INSERT INTO wishlist (user_id, videogame_id) VALUES (@UserId, @VideogameId);";
        public const string SQL_REMOVE_WISHLIST_VIDEOGAME = "DELETE FROM wishlist WHERE user_id = @UserId AND videogame_id = @VideogameId";
        public const string SQL_ADD_VIDEOGAME = "INSERT INTO videogames (title, developer, tag, description, price, release_date, on_sale) VALUES (@Title, @Developer, @Tag, @Description, @Price, @ReleaseDate, @OnSale)";
        public const string SQL_ADD_USER = "INSERT INTO appusers (username, email, password, active) VALUES (@Username, @Email, @Password, 1)";
        public const string SQL_ADD_USER_ADMIN = "INSERT INTO appusers (username, email, password, active) VALUES (@Username, @Email, @Password, @Active)";
        public const string SQL_UPDATE_VIDEOGAME = "UPDATE videogames SET title = @Title, developer = @Developer, tag = @Tag, description = @Description, price = @Price, release_date = @ReleaseDate, on_sale = @OnSale WHERE videogames.id = @VideogameId";
        public const string SQL_UPDATE_USER = "UPDATE appusers SET username = @Username, email = @Email, password = @Password, active = @Active WHERE id = @UserId";
        public const string SQL_CHECK_USER_EXISTS = "SELECT COUNT(id) AS 'users' FROM appusers WHERE email = @Email";
        public const string SQL_CHECK_USER_ACTIVE = "SELECT active FROM appusers WHERE email = @Email";
        public const string SQL_CHECK_USER_PWD = "SELECT u.id FROM appusers AS u WHERE u.email = @Email AND u.password = @Password";
        public const string SQL_CHECK_CART = "SELECT cart_id FROM cart WHERE user_id = @UserId AND status = 'active'";
        public const string SQL_CHECK_PRODUCT_IN_CART = "SELECT cart_product_id, quantity FROM cart_product WHERE cart_id = @CartId AND videogame_id = @VideogameId";
        public const string SQL_CREATE_CART = "INSERT INTO cart (user_id, status) VALUES (@UserId, 'active')";
        public const string SQL_CLEAR_CART = "UPDATE cart SET status = 'completed' WHERE user_id = @UserId AND cart_id = @CartId";
        public const string SQL_GET_CART = "SELECT cart_id FROM cart WHERE user_id = @UserId AND status = 'active'";
        public const string SQL_UPDATE_CART_PRODUCT = "UPDATE cart_product SET quantity = @Quantity WHERE cart_product_id = @CartProductId";
        public const string SQL_INSERT_CART_PRODUCT = "INSERT INTO cart_product (cart_id, videogame_id, quantity) VALUES (@CartId, @VideogameId, @Quantity)";
        public const string SQL_GET_CART_PRODUCTS = "SELECT cp.videogame_id, cp.quantity, v.title, v.developer, v.tag, v.description, v.price FROM cart_product AS cp JOIN videogames AS v ON v.id = cp.videogame_id WHERE cp.cart_id = @CartId";
        public const string SQL_GET_CART_PRODUCTS_FROM_DB = "SELECT videogame_id, quantity FROM cart_product WHERE cart_id = @CartId";
        public const string SQL_REMOVE_CART_PRODUCT = "DELETE FROM cart_product WHERE videogame_id = @VideogameId AND cart_id = @CartId";
        public const string SQL_CREATE_ORDER = "INSERT INTO orders (full_name, address, country, state, zip_code, card_number, ccv, card_owner, user_id, order_date) VALUES (@FullName, @Address, @Country, @State, @ZipCode, @CardNumber, @CCV, @CardOwner, @UserId, @OrderDate)";
        public const string SQL_CREATE_VIDEOGAME_ORDER = "INSERT INTO videogameorder (order_id, videogame_id, quantity) VALUES (@OrderId, @VideogameId, @Quantity)";
    }
}
