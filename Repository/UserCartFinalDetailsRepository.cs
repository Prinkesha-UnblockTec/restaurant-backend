using restaurant.Interfaces;
using restaurant.Models;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Net;
using static restaurant.Models.UserCartFinalDetails;
using System.Transactions;
using System.Reflection;

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
                        else if (model.OrderType == "Delivery")
                        {
                            cmd.Parameters.AddWithValue("@Status", "Ordered");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Status", "Accepted");
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
                        else if (model.OrderType == "Delivery")
                        {
                            cmd.Parameters.AddWithValue("@Status", "Ordered");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Status", "Inprogress");
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
                            cmd.Parameters.AddWithValue("@Checked", 0);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    if (model.OrderType != "Dine-in")
                    {
                        using (SqlCommand cmd = new SqlCommand("AddNotification", con, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@OrderId", newUserId);
                            cmd.Parameters.AddWithValue("@Description", "New Order is pending");
                            cmd.Parameters.AddWithValue("@IsRead", 0);
                            cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
                            cmd.Parameters.AddWithValue("@RoleName", "Admin");
                            cmd.Parameters.AddWithValue("@OrderType", model.OrderType);
                            cmd.Parameters.AddWithValue("@Status", 0);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    if (model.OrderType == "Delivery" || model.OrderType == "Pick-Up")
                    {
                        using (SqlCommand cmd = new SqlCommand("AddPaymentDetails", con, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@OrderId", newUserId);
                            cmd.Parameters.AddWithValue("@PaymentType", model.PaymentType);
                            //if (model.PaymentType == "credit" || model.PaymentType == "debit")
                            //{
                                cmd.Parameters.AddWithValue("@CardNumber", model.CardNumber);
                                cmd.Parameters.AddWithValue("@CardName", model.CardName);
                                cmd.Parameters.AddWithValue("@ExpireDate", model.ExpireDate);
                            //}else if (model.PaymentType == "upi")
                            //{
                                cmd.Parameters.AddWithValue("@UPIId", model.UPIId);
                            //}
                            //else if(model.PaymentType == "banking")
                            //{
                                cmd.Parameters.AddWithValue("@BankName", model.BankName);
                            //}
                            //else
                            //{

                            //}
                            cmd.ExecuteNonQuery();
                        }
                    }
                    if (model.OrderType == "Dine-in")
                    {
                        using (SqlCommand cmd = new SqlCommand("AssignChefsAndUpdateDetails", con, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@OrderId", newUserId);
                            cmd.Parameters.AddWithValue("@OrderType", model.OrderType);
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
                    cmd.Parameters.AddWithValue("@CartId", ID);

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrdersAdmin product = new OrdersAdmin();
                            product.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                            product.Checked = reader.GetBoolean(reader.GetOrdinal("Checked"));
                            product.Date = reader["Date"].ToString();
                            //product.Checked = reader["Checked"].ToString();
                            product.ImageURL = reader["ImageURL"].ToString();
                            product.ItemName = reader["ItemName"].ToString();
                            product.CategoriesName = reader["CategoriesName"].ToString();
                            product.Description = reader["Description"].ToString();
                            product.Currency = reader["Currency"].ToString();
                            product.ChefName = reader["ChefName"] != DBNull.Value ? reader["ChefName"].ToString() : string.Empty;
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
        public string GetChefNameById(int id)
        {
            string chefName = "";
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetStatusUsers", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@GetChefNameById", id));
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        chefName = reader["ChefName"].ToString();
                    }
                }
            }
            return chefName;
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

        public bool UpdateNotificationRoleName(UpdateRoleNameForNotification model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateRoleNameNotification", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NotificationId", model.Id);
                    cmd.Parameters.AddWithValue("@RoleName", model.RoleName);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        public bool UpdateNotificationIsRead(UpdateRoleNameForNotification model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateIsReadNotification", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NotificationId", model.Id);
                    cmd.Parameters.AddWithValue("@IsRead", 1);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public ICollection<Notification> GetAllNotificationData()
        {
            var RoleList = new List<Notification>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllNotification", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Notification Items = new Notification
                        {
                            Id = Convert.ToInt32(reader["ID"]),
                            OrderId = Convert.ToInt32(reader["OrderId"]),
                            IsRead = Convert.ToInt32(reader["IsRead"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            RoleName = reader["RoleName"].ToString(),
                            OrderType = reader["OrderType"].ToString(),
                            Description = reader["Description"].ToString(),
                            DateTime = Convert.ToDateTime(reader["DateTime"])
                        };
                        RoleList.Add(Items);

                    }
                }
            }
            return RoleList;
        }
        public bool UpdateNotification(Notification model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdatedNotification", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@RoleName", model.RoleName);
                    cmd.Parameters.AddWithValue("@Description", model.Description);
                    cmd.Parameters.AddWithValue("@Status", model.Status);
                    cmd.Parameters.AddWithValue("@IsRead", 0);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public bool UpdateCheckedItems(UpdateCheckedItems model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateCheckedItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@checked", model.Checked);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public bool DeleteCompleteOrderforNotification(int Id)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteNotificationData", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ID", Id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return true;
        }
        public void AssignChefsAndUpdateDetails(ModifyrelatableChef model)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AssignChefsAndUpdateDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parameters
                    cmd.Parameters.AddWithValue("@OrderId", model.orderId);
                    cmd.Parameters.AddWithValue("@OrderType", model.orderType);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool DeleteCheckedRTecordsInNotifications(string ids)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteCheckedRTecordsInNotifications", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Ids", ids);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return true;
        }

        public ICollection<Payment> GetPaymentAllData()
        {
            var PaymentList = new List<Payment>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetPaymentAllData", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Payment Items = new Payment
                        {
                            Id = reader["Id"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("Id")) : 0,
                            OrderID = reader["OrderID"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("OrderID")) : 0,
                            PaymentType = reader["PaymentType"] != DBNull.Value ? reader["PaymentType"].ToString() : string.Empty,
                            UPIId = reader["UPIId"] != DBNull.Value ? reader["UPIId"].ToString() : string.Empty,
                            CardNumber = reader["CardNumber"] != DBNull.Value ? reader["CardNumber"].ToString() : string.Empty,
                            CardName = reader["CardName"] != DBNull.Value ? reader["CardName"].ToString() : string.Empty,
                            ExpireDate = reader["ExpireDate"] != DBNull.Value ? reader["ExpireDate"].ToString() : string.Empty,
                            BankName = reader["BankName"] != DBNull.Value ? reader["BankName"].ToString() : string.Empty
                        };
                        PaymentList.Add(Items);

                    }
                }
            }
            return PaymentList;
        }

        public bool UpdatePaymentType(PaymentUpdate model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdatePaymentType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.Id);
                    cmd.Parameters.AddWithValue("@PaymentType", model.PaymentType);
                    cmd.Parameters.AddWithValue("@UPIId", model.UPIId);
                    cmd.Parameters.AddWithValue("@CardNumber", model.CardNumber);
                    cmd.Parameters.AddWithValue("@CardName", model.CardName);
                    cmd.Parameters.AddWithValue("@ExpireDate", model.ExpireDate);
                    cmd.Parameters.AddWithValue("@BankName", model.BankName);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public int GetTotalAmountByCartId(int cartId)
        {
            int totalAmount = 0;
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetTotalAmountByCartId", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@CartId", cartId));
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        totalAmount = Convert.ToInt32(reader["TotalAmount"]);
                    }
                }
            }
            return totalAmount;
        }
    }
}
