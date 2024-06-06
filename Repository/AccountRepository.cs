using Microsoft.Extensions.Configuration;
using restaurant.Interfaces;
using restaurant.Models;
using System.Data.SqlClient;
using System.Data;

namespace restaurant.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _configuration;

        public AccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool GetActiveMenu(Menu menu)
        {
            throw new NotImplementedException();
        }
        public bool AddedRegisterList(Login model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SaveRegisterData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", model.UserName);
                    cmd.Parameters.AddWithValue("@Password", model.Password);
                    cmd.Parameters.AddWithValue("@Role", model.Role);
                    cmd.Parameters.AddWithValue("@Status", model.Status);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        public (bool IsValidUser, int? NewUserId) UserLogin(Login user)
        {
            int? newUserId = null;
            bool isValidUser = false;

            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("CheckUserCredentials", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    SqlParameter outputValidUserParam = new SqlParameter("@IsValidUser", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputValidUserParam);

                    SqlParameter outputIdParam = new SqlParameter("@LoginId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputIdParam);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    isValidUser = Convert.ToBoolean(outputValidUserParam.Value);
                    if (outputIdParam.Value != DBNull.Value)
                    {
                        newUserId = Convert.ToInt32(outputIdParam.Value);
                    }
                }
            }

            return (isValidUser, newUserId);
        }

        public ICollection<User> GetLoginUserDataBaseOnID(int Id)
        {
            var UserList = new List<User>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("getAllUserDatas", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@ID", Id));
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User Users = new User
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            UserName = reader["UserName"].ToString(),
                            RoleID = reader["RoleID"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("RoleID")) : 0,
                            Role = reader["Role"] != DBNull.Value ? reader["Role"].ToString() : string.Empty,
                            Password = reader["Password"] != DBNull.Value ? reader["Password"].ToString() : string.Empty,
                            Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty,
                        };
                        UserList.Add(Users);
                    }
                }
            }
            return UserList;
        }
        public bool SetDefaultRouting(UpdateRouting model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SetDefaultRouting", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@RouteName", model.RouteName);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public ICollection<UpdateRouting> GetDefaultRouting()
        {
            var RoutingList = new List<UpdateRouting>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDefaultRouting", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UpdateRouting items = new UpdateRouting
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            RouteName = reader["RouteName"].ToString(),
                        };
                        RoutingList.Add(items);

                    }
                }
            }
            return RoutingList;
        }
        public bool UpdateCurrecyAdmin(UpdateCurrency model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateCurrency", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.Id);
                    cmd.Parameters.AddWithValue("@Currency", model.Currency);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        public string GetCurrecyAdmin()
        {
            string currency = "" ;
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetCurrency", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currency = reader["Currency"].ToString();
                    }
                }
            }
            return currency;
        }
        private int RetrieveUserIdByUser(string UserName)
        {
            int userId = 0;
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetIdByEmail", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserName", UserName);
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userId = Convert.ToInt32(reader["ID"]);
                    }
                }
            }

            return userId;
        }

        public ICollection<Role> GetAllUserWithRoleData()
        {
            var RoleList = new List<Role>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllRoleData", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Role Roles = new Role
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            RoleName = reader["RoleName"].ToString(),
                            Status = reader["Status"].ToString(),
                        };
                        RoleList.Add(Roles);

                    }
                }
            }
            return RoleList;
        }

        public ICollection<LoginInfo> GetLoginIdByUsernameAndPasswordAsync(string userName, string password)
        {
            List<LoginInfo> logins = new List<LoginInfo>();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("getIdbaseonUserNameandpassword", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@Password", password);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            int loginId = reader.GetInt32(0);
                            string tableName = reader.GetString(1);

                            // Assuming Login has a constructor that takes loginId and tableName
                            LoginInfo login = new LoginInfo(loginId, tableName);
                            logins.Add(login);
                        }
                    }
                }
            }

            return logins;
        }



        public int Login(Login user)
        {
            int loginResult = 0;
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("loginUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    SqlParameter resultParam = new SqlParameter("@LoginResult", SqlDbType.Int);
                    resultParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(resultParam);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    loginResult = Convert.ToInt32(cmd.Parameters["@LoginResult"].Value);
                }
            }

            return loginResult;
        }



        public bool UpdateLogin(loginwithallDetails model)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("updatelogin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LoginId", model.loginId);
                    cmd.Parameters.AddWithValue("@UserName", string.IsNullOrEmpty(model.userName) ? (object)DBNull.Value : model.userName);
                    cmd.Parameters.AddWithValue("@Password", string.IsNullOrEmpty(model.password) ? (object)DBNull.Value : model.password);
                    cmd.Parameters.AddWithValue("@TableName", model.tableName);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        int IAccountRepository.RetrieveUserIdByUser(string UserName)
        {
            throw new NotImplementedException();
        }

        public ICollection<OrderTypes> GetOrderTypes()
        {
            var OrderTypesList = new List<OrderTypes>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetOrderTypes", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OrderTypes item = new OrderTypes
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            OrderType = reader["OrderType"].ToString(),
                            Status = reader["Status"].ToString(),
                        };
                        OrderTypesList.Add(item);

                    }
                }
            }
            return OrderTypesList;
        }

        public async Task UpdateOrderTypeStatusAsync(string selectedOrderTypes)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateOrderTypeStatus", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@SelectedOrderTypes", SqlDbType.NVarChar)
                    {
                        Value = selectedOrderTypes
                    });

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        //private int RetrieveUserIdByEmail(string UserName)
        //{
        //    int userId = 0;
        //    string? connectionString = _configuration.GetConnectionString("DefaultConnection");

        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("GetIdByEmail", con)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };
        //        cmd.Parameters.AddWithValue("@UserName", UserName);
        //        con.Open();

        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                userId = Convert.ToInt32(reader["ID"]);
        //            }
        //        }
        //    }

        //    return userId;
        //}



        //public ICollection<User> getAllUserList(int loginId)
        //{
        //    throw new NotImplementedException();
        //}

        //public int GetRoleIdBaseOnLoginEamil(int LoginId)
        //{
        //    int RoleID = 0;
        //    string? connectionString = _configuration.GetConnectionString("DefaultConnection");

        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("GetLogiIdBaseGetRoleID", con)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };
        //        cmd.Parameters.AddWithValue("@ID", LoginId);
        //        con.Open();

        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                RoleID = Convert.ToInt32(reader["RoleID"]);
        //            }
        //        }
        //    }

        //    return RoleID;
        //}
    }
}