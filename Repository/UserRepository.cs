using restaurant.Interfaces;
using System.Data.SqlClient;
using System.Data;
using restaurant.Models;

namespace restaurant.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ICollection<User> GetAllUserData()
        {
            var UserList = new List<User>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllUserData", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User Users = new User
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            UserName = reader["UserName"].ToString(),
                            RoleID = Convert.ToInt32(reader["RoleID"]),
                            RoleName = reader["RoleName"].ToString(),
                            Password = reader["Password"].ToString(),
                        };
                        UserList.Add(Users);

                    }
                }
            }
            return UserList;
        }
        public bool AddedUserList(User model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddedUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", model.UserName);
                    cmd.Parameters.AddWithValue("@Password", model.Password);
                    cmd.Parameters.AddWithValue("@RoleID", model.RoleID);
                    cmd.Parameters.AddWithValue("@RoleName", model.RoleName);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        public bool EditUser(User model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EditUserData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@UserName", model.UserName);
                    cmd.Parameters.AddWithValue("@Password", model.Password);
                    cmd.Parameters.AddWithValue("@RoleID", model.RoleID);
                    cmd.Parameters.AddWithValue("@RoleName", model.RoleName);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        public bool DeleteUser(int id)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteUser", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return true;
        }
    }
}