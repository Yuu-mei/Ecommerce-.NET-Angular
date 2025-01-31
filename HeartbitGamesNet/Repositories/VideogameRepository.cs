using HeartbitGamesNet.Constants;
using HeartbitGamesNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Text.Json;

namespace HeartbitGamesNet.Repositories
{
    public class VideogameRepository : IVideogameRepository
    {
        private readonly string _connectionString;

        public VideogameRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public string AddVideogame(Videogame v)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            using(MySqlCommand cmd = new(SQLConstants.SQL_ADD_VIDEOGAME, conn))
            {
                cmd.Parameters.AddWithValue("@Title", v.Title);
                cmd.Parameters.AddWithValue("@Developer", v.Developer);
                cmd.Parameters.AddWithValue("@Tag", v.Tag);
                cmd.Parameters.AddWithValue("@Description", v.Description);
                cmd.Parameters.AddWithValue("@Price", v.Price);
                cmd.Parameters.AddWithValue("@ReleaseDate", v.ReleaseDate);
                cmd.Parameters.AddWithValue("@OnSale", v.OnSale);
                
                int rowsAffected = cmd.ExecuteNonQuery();

                //Generated ID
                long gId = cmd.LastInsertedId;
                conn.Close();

                //Upload imgs
                FormFile headerImg = (FormFile) v.HeaderImg;
                FormFile capsuleImg = (FormFile) v.CapsuleImg;

                string headerImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "header");
                string capsuleImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "capsule");

                string headerImgFilename = Path.Combine(headerImgPath, gId.ToString() + ".jpg");
                string capsuleImgFilename = Path.Combine(capsuleImgPath, gId.ToString() + ".jpg");

                using (FileStream stream = new(headerImgFilename, FileMode.Create))
                {
                    headerImg.CopyTo(stream);
                }
                using (FileStream stream = new(capsuleImgFilename, FileMode.Create))
                {
                    capsuleImg.CopyTo(stream);
                }


                //Res
                if (rowsAffected > 0)
                {
                    return "OK";
                }
                else
                {
                    return "ERROR";
                }
            }
        }

        public string EditVideogame(Videogame v)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            using (MySqlCommand cmd = new(SQLConstants.SQL_UPDATE_VIDEOGAME, conn))
            {
                cmd.Parameters.AddWithValue("@Title", v.Title);
                cmd.Parameters.AddWithValue("@Developer", v.Developer);
                cmd.Parameters.AddWithValue("@Tag", v.Tag);
                cmd.Parameters.AddWithValue("@Description", v.Description);
                cmd.Parameters.AddWithValue("@Price", v.Price);
                cmd.Parameters.AddWithValue("@ReleaseDate", v.ReleaseDate);
                cmd.Parameters.AddWithValue("@OnSale", v.OnSale);
                cmd.Parameters.AddWithValue("@VideogameId", v.Id);

                int rowsAffected = cmd.ExecuteNonQuery();

                conn.Close();

                //Upload imgs
                FormFile headerImg = (FormFile)v.HeaderImg;
                FormFile capsuleImg = (FormFile)v.CapsuleImg;

                string headerImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "header");
                string capsuleImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "capsule");

                string headerImgFilename = Path.Combine(headerImgPath, v.Id.ToString() + ".jpg");
                string capsuleImgFilename = Path.Combine(capsuleImgPath, v.Id.ToString() + ".jpg");

                using (FileStream stream = new(headerImgFilename, FileMode.Create))
                {
                    headerImg.CopyTo(stream);
                }
                using (FileStream stream = new(capsuleImgFilename, FileMode.Create))
                {
                    capsuleImg.CopyTo(stream);
                }


                //Res
                if (rowsAffected > 0)
                {
                    return "OK";
                }
                else
                {
                    return "ERROR";
                }
            }
        }

        public List<Videogame> GetAllVideogames()
        {
            List<Videogame> VideogameList = new List<Videogame>();
            MySqlConnection conn = GetConnection();
            conn.Open();

            MySqlCommand cmd = new(SQLConstants.SQL_OBTAIN_ALL_VIDEOGAMES, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) {
                VideogameList.Add(new Videogame
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Title = reader["title"].ToString()!,
                    Developer = reader["developer"].ToString()!,
                    Tag = reader["tag"].ToString()!,
                    Description = reader["description"].ToString()!,
                    Price = Convert.ToDouble(reader["price"]),
                    ReleaseDate = Convert.ToDateTime(reader["release_date"]),
                    OnSale = Convert.ToBoolean(reader["on_sale"])
                });
            }

            reader.Close();
            conn.Close();

            return VideogameList;
        }

        public List<Videogame> GetLatestVideogames()
        {
            List<Videogame> VideogameList = new List<Videogame>();
            MySqlConnection conn = GetConnection();
            conn.Open();

            MySqlCommand cmd = new(SQLConstants.SQL_OBTAIN_LATEST_VIDEOGAMES, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                VideogameList.Add(new Videogame
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Title = reader["title"].ToString()!,
                    Developer = reader["developer"].ToString()!,
                    Tag = reader["tag"].ToString()!,
                    Description = reader["description"].ToString()!,
                    Price = Convert.ToDouble(reader["price"]),
                    ReleaseDate = Convert.ToDateTime(reader["release_date"]),
                    OnSale = Convert.ToBoolean(reader["on_sale"])
                });
            }

            reader.Close();
            conn.Close();

            return VideogameList;
        }

        public List<Videogame> GetSimilarVideogames(string videogame_id, string developer, string tag)
        {
            List<Videogame> VideogameList = new List<Videogame>();
            MySqlConnection conn = GetConnection();
            conn.Open();

            MySqlCommand cmd = new(SQLConstants.SQL_OBTAIN_SIMILAR_VIDEOGAMES, conn);
            //Parameters
            cmd.Parameters.AddWithValue("@Tag", tag);
            cmd.Parameters.AddWithValue("@Developer", developer);
            cmd.Parameters.AddWithValue("@Id", videogame_id);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                VideogameList.Add(new Videogame
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Title = reader["title"].ToString()!,
                    Developer = reader["developer"].ToString()!,
                    Tag = reader["tag"].ToString()!,
                    Description = reader["description"].ToString()!,
                    Price = Convert.ToDouble(reader["price"]),
                    ReleaseDate = Convert.ToDateTime(reader["release_date"]),
                    OnSale = Convert.ToBoolean(reader["on_sale"])
                });
            }

            reader.Close();
            conn.Close();

            return VideogameList;
        }

        public List<string> GetAllTags()
        {
            MySqlConnection conn = GetConnection();
            conn.Open();

            MySqlCommand cmd = new(SQLConstants.SQL_OBTAIN_ALL_TAGS, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            List<string> TagList = new List<string>();

            while (reader.Read())
            {
                TagList.Add(reader["tag"].ToString());
            }

            reader.Close();
            conn.Close();

            return TagList;
        }

        public List<string> GetAllDevs()
        {
            MySqlConnection conn = GetConnection();
            conn.Open();

            MySqlCommand cmd = new(SQLConstants.SQL_OBTAIN_ALL_DEVS, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            List<string> DevList = new List<string>();

            while (reader.Read())
            {
                DevList.Add(reader["developer"].ToString()!);
            }

            reader.Close();
            conn.Close();

            return DevList;
        }

        public List<Videogame> Search(string dev = "", string tag = "", string title = "")
        {
            MySqlConnection conn = GetConnection();
            conn.Open();

            //Building the SQL
            string final_sql = SQLConstants.SQL_BASE_SEARCH;
            if (!dev.IsNullOrEmpty())
            {
                final_sql += " AND LOWER(developer) LIKE LOWER(@Dev)";
            }
            if (!tag.IsNullOrEmpty()) {
                final_sql += " AND LOWER(tag) LIKE LOWER(@Tag)";
            }
            if (!title.IsNullOrEmpty())
            {
                final_sql += " AND LOWER(title) LIKE LOWER(@Title)";
            }

            //There has to be a better way than doing this
            MySqlCommand cmd = new(final_sql, conn);
            if (!dev.IsNullOrEmpty())
            {
                cmd.Parameters.AddWithValue("Dev", $"%{dev}%");
            }
            if (!tag.IsNullOrEmpty())
            {
                cmd.Parameters.AddWithValue("Tag", $"%{tag}%");
            }
            if (!title.IsNullOrEmpty())
            {
                cmd.Parameters.AddWithValue("Title", $"%{title}%");
            }

            MySqlDataReader reader = cmd.ExecuteReader();
            List<Videogame> VideogameList = new List<Videogame>();

            while (reader.Read()) 
            {
                VideogameList.Add(new Videogame
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Title = reader["title"].ToString()!,
                    Developer = reader["developer"].ToString()!,
                    Tag = reader["tag"].ToString()!,
                    Description = reader["description"].ToString()!,
                    Price = Convert.ToDouble(reader["price"]),
                    ReleaseDate = Convert.ToDateTime(reader["release_date"]),
                    OnSale = Convert.ToBoolean(reader["on_sale"])
                });
            }

            reader.Close();
            conn.Close();

            return VideogameList;
        }

        public Dictionary<string, List<string>> GetFilters()
        {
            List<string> tags = GetAllTags();
            List<string> devs = GetAllDevs();

            Dictionary<string, List<string>> filters = new Dictionary<string, List<string>>();
            filters.Add("tags", tags);
            filters.Add("devs", devs);

            return filters;
        }

        public List<Videogame> GetAllVideogamesAdmin()
        {
            List<Videogame> VideogameList = new List<Videogame>();
            MySqlConnection conn = GetConnection();
            conn.Open();

            MySqlCommand cmd = new(SQLConstants.SQL_OBTAIN_ALL_VIDEOGAMES_ADMIN, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                VideogameList.Add(new Videogame
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Title = reader["title"].ToString()!,
                    Developer = reader["developer"].ToString()!,
                    Tag = reader["tag"].ToString()!,
                    Description = reader["description"].ToString()!,
                    Price = Convert.ToDouble(reader["price"]),
                    ReleaseDate = Convert.ToDateTime(reader["release_date"]),
                    OnSale = Convert.ToBoolean(reader["on_sale"])
                });
            }

            reader.Close();
            conn.Close();

            return VideogameList;
        }

        public Videogame GetVideogameDetail(int id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();

            MySqlCommand cmd = new(SQLConstants.SQL_OBTAIN_VIDEOGAME_DETAIL, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            MySqlDataReader reader = cmd.ExecuteReader();

            Videogame v = null;

            if (reader.Read())
            {
                v = new Videogame
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Title = reader["title"].ToString()!,
                    Developer = reader["developer"].ToString()!,
                    Tag = reader["tag"].ToString()!,
                    Description = reader["description"].ToString()!,
                    Price = Convert.ToDouble(reader["price"]),
                    ReleaseDate = Convert.ToDateTime(reader["release_date"]),
                    OnSale = Convert.ToBoolean(reader["on_sale"])
                };
            }

            conn.Close();
            reader.Close();

            return v;
        }

        public string DeleteVideogameById(int id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();

            MySqlCommand cmd = new(SQLConstants.SQL_DELETE_VIDEOGAME, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();

            //Delete image
            string headerImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "header");
            string capsuleImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "capsule");

            string headerImgFilename = Path.Combine(headerImgPath, id.ToString() + ".jpg");
            string capsuleImgFilename = Path.Combine(capsuleImgPath, id.ToString() + ".jpg");

            File.Delete(headerImgFilename);
            File.Delete(capsuleImgFilename);

            conn.Close();

            return "ok";
        }

        public string DeactivateVideogameById(int id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();

            MySqlCommand cmd = new(SQLConstants.SQL_DEACTIVATE_VIDEOGAME, conn);
            cmd.Parameters.AddWithValue("@VideogameId", id);
            cmd.ExecuteNonQuery();

            return "ok";
        }

        public string WishlistVideogame(string videogame_id, string user_id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();

            MySqlCommand cmdCheckWishlist = new(SQLConstants.SQL_CHECK_VIDEOGAME_WISHLISTED, conn);
            cmdCheckWishlist.Parameters.AddWithValue("@UserId", user_id);
            cmdCheckWishlist.Parameters.AddWithValue("@VideogameId", videogame_id);

            string res = cmdCheckWishlist.ExecuteScalar().ToString()!;
            if(res == "0")
            {
                MySqlCommand addWishlist = new(SQLConstants.SQL_ADD_WISHLIST_VIDEOGAME, conn);
                addWishlist.Parameters.AddWithValue("@UserId", user_id);
                addWishlist.Parameters.AddWithValue("@VideogameId", videogame_id);
                addWishlist.ExecuteNonQuery();
                conn.Close();
                return JsonSerializer.Serialize("ok");
            }
            else
            {
                MySqlCommand removeWishlist = new(SQLConstants.SQL_REMOVE_WISHLIST_VIDEOGAME, conn);
                removeWishlist.Parameters.AddWithValue("@UserId", user_id);
                removeWishlist.Parameters.AddWithValue("@VideogameId", videogame_id);
                removeWishlist.ExecuteNonQuery();
                conn.Close();
                return JsonSerializer.Serialize("undo-wishlist");
            }
        }

        public string IsVideogameWishlisted(string videogame_id, string user_id)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();

            MySqlCommand cmdCheckWishlist = new(SQLConstants.SQL_CHECK_VIDEOGAME_WISHLISTED, conn);
            cmdCheckWishlist.Parameters.AddWithValue("@UserId", user_id);
            cmdCheckWishlist.Parameters.AddWithValue("@VideogameId", videogame_id);

            string res = cmdCheckWishlist.ExecuteScalar().ToString()!;
            if (res == "0")
            {
                conn.Close();
                return JsonSerializer.Serialize("no");
            }
            else
            {
                conn.Close();
                return JsonSerializer.Serialize("yes");
            }
        }
    }
}
