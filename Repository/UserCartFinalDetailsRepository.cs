using restaurant.Interfaces;
using restaurant.Models;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Net;
using static restaurant.Models.UserCartFinalDetails;
using System.Transactions;

namespace restaurant.Repository
{
    public class UserCartFinalDetailsRepository : IUserCartFinalDetails
    {
        private readonly IConfiguration _configuration;

        public UserCartFinalDetailsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public bool AddedUserCartList(UserCartFinalDetails.CartDetails model)
        //{
        //    int newUserId;
        //    string? connectionString = _configuration.GetConnectionString("DefaultConnection");
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("addedCartUserDefultAddress", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@LoginId", model.LoginId);
        //            cmd.Parameters.AddWithValue("@UserName", model.Username);
        //            cmd.Parameters.AddWithValue("@Address", model.Address);
        //            cmd.Parameters.AddWithValue("@City", model.City);
        //            cmd.Parameters.AddWithValue("@State", model.State);
        //            cmd.Parameters.AddWithValue("@PinCode", model.PinCode);
        //            cmd.Parameters.AddWithValue("@Time", model.Time);
        //            cmd.Parameters.AddWithValue("@Currency", model.Currency);
        //            cmd.Parameters.AddWithValue("@Date", model.Date);
        //            cmd.Parameters.AddWithValue("@DeliveryName", model.DeliverName);
        //            SqlParameter outputIdParam = new SqlParameter("@NewUserID", SqlDbType.Int);
        //            outputIdParam.Direction = ParameterDirection.Output;
        //            cmd.Parameters.Add(outputIdParam);

        //            con.Open();
        //            cmd.ExecuteNonQuery();

        //            newUserId = Convert.ToInt32(outputIdParam.Value);

        //        }
        //        using (SqlCommand cmd = new SqlCommand("LogStatusChange", con, transaction))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@CartId", newUserId);
        //            cmd.Parameters.AddWithValue("@Status", "Pending");
        //            cmd.ExecuteNonQuery();
        //        }
        //        foreach (var product in model.Products)
        //        {
        //            string? connectionStrings = _configuration.GetConnectionString("DefaultConnection");
        //            using (SqlConnection connection = new SqlConnection(connectionStrings))
        //            {
        //                using (SqlCommand cmd = new SqlCommand("AddedItemsDetails", connection))
        //                {
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.AddWithValue("@CartId", newUserId);
        //                    cmd.Parameters.AddWithValue("@UserCartId", model.Username);
        //                    cmd.Parameters.AddWithValue("@Date", model.Date);
        //                    cmd.Parameters.AddWithValue("@ItemName", product.ItemName);
        //                    cmd.Parameters.AddWithValue("@Price", product.Price);
        //                    cmd.Parameters.AddWithValue("@CategoriesName", product.CategoriesName);
        //                    cmd.Parameters.AddWithValue("@Description", product.Description);
        //                    cmd.Parameters.AddWithValue("@Currency", model.Currency);
        //                    cmd.Parameters.AddWithValue("@ImageURL", product.ImageURL);
        //                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
        //                    connection.Open();
        //                    cmd.ExecuteNonQuery();
        //                }
        //            }
        //        }
        //    }
        //    return true;
        //}

        public bool AddedUserCartList(UserCartFinalDetails.CartDetails model)
        {
            int newUserId;
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    // Add user cart details
                    using (SqlCommand cmd = new SqlCommand("addedCartUserDefultAddress", con, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LoginId", model.LoginId);
                        cmd.Parameters.AddWithValue("@UserName", model.Username);
                        cmd.Parameters.AddWithValue("@TableNo", model.TableNo);
                        cmd.Parameters.AddWithValue("@OrderType", model.OrderType);
                        cmd.Parameters.AddWithValue("@Address", model.Address);
                        cmd.Parameters.AddWithValue("@City", model.City);
                        cmd.Parameters.AddWithValue("@State", model.State);
                        cmd.Parameters.AddWithValue("@PinCode", model.PinCode);
                        cmd.Parameters.AddWithValue("@Time", model.Time);
                        cmd.Parameters.AddWithValue("@Currency", model.Currency);
                        cmd.Parameters.AddWithValue("@Date", model.Date);
                        cmd.Parameters.AddWithValue("@DeliveryName", model.DeliverName);
                        if (model.OrderType == "Pick-Up")
                        {
                            cmd.Parameters.AddWithValue("@Status", "Accepted");
                        }
                        else if(model.OrderType == "Delivery")
                        {
                            cmd.Parameters.AddWithValue("@Status", "Ordered");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Status", "Inprogress");
                        }
                        SqlParameter outputIdParam = new SqlParameter("@NewUserID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdParam);
                        cmd.ExecuteNonQuery();
                        newUserId = Convert.ToInt32(outputIdParam.Value);
                    }

                    // Log status change for the new user cart
                    using (SqlCommand cmd = new SqlCommand("LogStatusChange", con, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CartId", newUserId);
                        if (model.OrderType == "Pick-Up")
                        {
                            cmd.Parameters.AddWithValue("@Status", "Accepted");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Status", "Ordered");
                        }
                        cmd.ExecuteNonQuery();
                    }

                    // Add each product to the cart
                    foreach (var product in model.Products)
                    {
                        using (SqlCommand cmd = new SqlCommand("AddedItemsDetails", con, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CartId", newUserId);
                            cmd.Parameters.AddWithValue("@UserCartId", model.Username);
                            cmd.Parameters.AddWithValue("@TableNo", model.TableNo);
                            cmd.Parameters.AddWithValue("@Date", model.Date);
                            cmd.Parameters.AddWithValue("@ItemName", product.ItemName);
                            cmd.Parameters.AddWithValue("@Price", product.Price);
                            cmd.Parameters.AddWithValue("@CategoriesName", product.CategoriesName);
                            cmd.Parameters.AddWithValue("@Description", product.Description);
                            cmd.Parameters.AddWithValue("@Currency", model.Currency);
                            cmd.Parameters.AddWithValue("@ImageURL", product.ImageURL);
                            cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
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
                    cmd.Parameters.AddWithValue("@LoginId", user.loginId);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Items product = new Items();
                            product.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                            product.LoginId = reader.GetInt32(reader.GetOrdinal("LoginId"));
                            product.Date = reader["Date"].ToString();
                            product.Time = reader["Time"].ToString();
                            product.UserName = reader["Username"].ToString();
                            product.Currency = reader["Currency"].ToString();
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
                            product.Currency = reader["Currency"].ToString();
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
                        product.OrderType = reader["OrderType"] != DBNull.Value ? reader["OrderType"].ToString() : string.Empty; 
                        product.Time = reader["Time"].ToString();
                        product.Status = reader["Status"].ToString();
                        product.Currency = reader["Currency"].ToString();
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
                    cmd.Parameters.AddWithValue("@Status", model.Status);
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
                        item.Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : string.Empty;
                        item.State = reader["State"] != DBNull.Value ? reader["State"].ToString() : string.Empty;
                        item.City = reader["City"] != DBNull.Value ? reader["City"].ToString() : string.Empty;
                        item.PinCode = reader["PinCode"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("PinCode")) : 0;
                        item.DeliveryName = reader["DeliveryName"] != DBNull.Value ? reader["DeliveryName"].ToString() : string.Empty;
                        addresArray.Add(item);
                    }
                }
            }

            return addresArray;
        }

        public ICollection<DeliveryAddress> GetAddressByOrderId(int Id)
        {
            List<DeliveryAddress> addresArray = new List<DeliveryAddress>();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAddressByOrderId", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@Id", Id));

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DeliveryAddress item = new DeliveryAddress();
                        item.Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : string.Empty;
                        item.State = reader["State"] != DBNull.Value ? reader["State"].ToString() : string.Empty;
                        item.City = reader["City"] != DBNull.Value ? reader["City"].ToString() : string.Empty;
                        item.PinCode = reader["PinCode"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("PinCode")) : 0;
                        item.DeliveryName = reader["DeliveryName"] != DBNull.Value ? reader["DeliveryName"].ToString() : string.Empty;
                        addresArray.Add(item);
                    }
                }
            }

            return addresArray;
        }

        public string GetStatusUser(int id)
        {
            string Status = "";
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetStatusUsers", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@ID", id));
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Status = reader["Status"].ToString();
                    }
                }
            }
            return Status;
        }

        public bool StoreSatausData(StoreSatausData model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("LogStatusChange", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CartId", model.ID);
                    cmd.Parameters.AddWithValue("@Status", model.Status);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public ICollection<StoreSatausData> GetStoreSatausData(int ID)
        {
            var StatusList = new List<StoreSatausData>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetLogStatusChange", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@CartId", ID));
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StoreSatausData item = new StoreSatausData
                        {
                            ID = Convert.ToInt32(reader["CartId"]),
                            Status = reader["Status"].ToString(),
                            DateTime = Convert.ToDateTime(reader["DateTime"])
                        };
                        StatusList.Add(item);

                    }
                }
            }
            return StatusList;
        }



        public string GetLastSelectedOrderType()
        {
            string orderType = "";
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetLastSelectedOrderType", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        orderType = reader["OrderType"].ToString();
                    }
                }
            }
            return orderType;
        }

        public bool AddedDataBaseCartId(CartModel model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    foreach (var product in model.Products)
                    {
                        using (SqlCommand cmd = new SqlCommand("AddedItemsDetails", con, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CartId", model.CartId);
                            cmd.Parameters.AddWithValue("@UserCartId", model.UserCartId);
                            cmd.Parameters.AddWithValue("@TableNo", model.TableNo);
                            cmd.Parameters.AddWithValue("@Date", model.Date);
                            cmd.Parameters.AddWithValue("@ItemName", product.ItemName);
                            cmd.Parameters.AddWithValue("@Price", product.Price);
                            cmd.Parameters.AddWithValue("@CategoriesName", product.CategoriesName);
                            cmd.Parameters.AddWithValue("@Description", product.Description);
                            cmd.Parameters.AddWithValue("@Currency", model.Currency);
                            cmd.Parameters.AddWithValue("@ImageURL", product.ImageURL);
                            cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
            }
            return true;
        }
    }
}
