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