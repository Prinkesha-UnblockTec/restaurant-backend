using restaurant.Interfaces;
using restaurant.Models;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Net;
using static restaurant.Models.UserCartFinalDetails;

namespace restaurant.Repository
{
    public class UserCartFinalDetailsRepository : IUserCartFinalDetails
    {
        private readonly IConfiguration _configuration;

        public UserCartFinalDetailsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool AddedUserCartList(UserCartFinalDetails.CartDetails model)
        {
            int newUserId;
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddedUserDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", model.Username);
                    cmd.Parameters.AddWithValue("@Password", model.Password);
                    cmd.Parameters.AddWithValue("@Address", model.Address);
                    cmd.Parameters.AddWithValue("@City", model.City);
                    cmd.Parameters.AddWithValue("@State", model.State);
                    cmd.Parameters.AddWithValue("@PinCode", model.PinCode);
                    cmd.Parameters.AddWithValue("@TabelName", model.TabelName);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    SqlParameter outputIdParam = new SqlParameter("@NewUserID", SqlDbType.Int);
                    outputIdParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputIdParam);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    newUserId = Convert.ToInt32(outputIdParam.Value);

                }
                foreach (var product in model.Products)
                {
                    string? connectionStrings = _configuration.GetConnectionString("DefaultConnection");
                    using (SqlConnection connection = new SqlConnection(connectionStrings))
                    {
                        using (SqlCommand cmd = new SqlCommand("AddedItemsDetails", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CartId", newUserId);
                            cmd.Parameters.AddWithValue("@UserCartId", model.Username);
                            cmd.Parameters.AddWithValue("@Date", model.Date);
                            cmd.Parameters.AddWithValue("@ItemName", product.ItemName);
                            cmd.Parameters.AddWithValue("@Price", product.Price);
                            cmd.Parameters.AddWithValue("@CategoriesName", product.CategoriesName);
                            cmd.Parameters.AddWithValue("@Description", product.Description);
                            cmd.Parameters.AddWithValue("@ImageURL", product.ImageURL);
                            cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                            connection.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            return true;
        }
        public ICollection<Items> GetUserCartProductsByTableName(ItemDataBseOnUser user)
        {
            List<Items> products = new List<Items>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetUserCartProductsByTableName", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cartId", user.cartId);
                    cmd.Parameters.AddWithValue("@tableName", user.tableName);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Items product = new Items();
                            product.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                            product.Date = reader["Date"].ToString();
                            product.Status = reader["Status"].ToString();
                            product.Price = reader.GetInt32(reader.GetOrdinal("TotalPrice"));
                            product.Quantity = reader.GetInt32(reader.GetOrdinal("TotalQuantity"));

                            products.Add(product);
                        }
                    }
                }
            }

            return products;
        }

        public ICollection<OrdersAdmin> GetOrdersInAdminData(int ID)
        {
            List<OrdersAdmin> products = new List<OrdersAdmin>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetOrdersInAdmin", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CartId",ID);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrdersAdmin product = new OrdersAdmin();
                            product.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                            product.Date = reader["Date"].ToString();
                            product.ImageURL = reader["ImageURL"].ToString();
                            product.ItemName = reader["ItemName"].ToString();
                            product.CategoriesName = reader["CategoriesName"].ToString();
                            product.Description = reader["Description"].ToString();
                            product.Price = reader.GetInt32(reader.GetOrdinal("Price"));
                            product.Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));

                            products.Add(product);
                        }
                    }
                }
            }

            return products;
        }


        public ICollection<AllCartItems> GetAllCartItems()
        {
            List<AllCartItems> products = new List<AllCartItems>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("GetAllCartItems", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AllCartItems product = new AllCartItems();
                        product.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                        product.Date = reader["Date"].ToString();
                        product.UserName = reader["UserName"].ToString();
                        product.Password = reader["Password"].ToString();
                        product.Status = reader["Status"].ToString();
                        product.TabelName = reader["TabelName"].ToString();
                        product.Price = reader.GetInt32(reader.GetOrdinal("TotalPrice"));
                        product.Quantity = reader.GetInt32(reader.GetOrdinal("TotalQuantity"));

                        products.Add(product);
                    }

                }
            }

            return products;
        }
        public bool UpdateParticularUserStatus(UpdateStatusUserCart model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateStatusByUsernamePasswordTableName", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Username", model.UserName);
                    cmd.Parameters.AddWithValue("@Password", model.Password);
                    cmd.Parameters.AddWithValue("@Status", model.Status);
                    cmd.Parameters.AddWithValue("@TabelName", model.TabelName);
                    cmd.Parameters.AddWithValue("@Date", model.Date);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        public ICollection<DeliveryAddress> GetAddressByUsernamePassword(string username, string password)
        {
            List<DeliveryAddress> addresArray = new List<DeliveryAddress>();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAddressByUsernamePassword", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@username", username));
                cmd.Parameters.Add(new SqlParameter("@password", password));

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DeliveryAddress item = new DeliveryAddress();
                        item.Address = reader["Address"].ToString();
                        item.State = reader["State"].ToString();
                        item.City = reader["City"].ToString();
                        item.PinCode = reader.GetInt32(reader.GetOrdinal("PinCode"));


                        addresArray.Add(item);
                    }
                }
            }

            return addresArray;
        }


    }
}
