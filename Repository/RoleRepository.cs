using Microsoft.Extensions.Configuration;
using restaurant.Interfaces;
using restaurant.Models;
using System.Data.SqlClient;
using System.Data;

namespace restaurant.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IConfiguration _configuration;

        public RoleRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ICollection<Role> GetAllRoleData()
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
        public bool AddedRoleList(Role model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddRoleData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RoleName", model.RoleName);
                    cmd.Parameters.AddWithValue("@Status", model.Status);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        public bool EditRole(Role model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EditRoleData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@RoleName", model.RoleName);
                    cmd.Parameters.AddWithValue("@Status", model.Status);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public bool DeleteRole(int id)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteRole", con)
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
        ICollection<ActiveRole> IRoleRepository.GetActiveRole()
        {
            var RoleList = new List<ActiveRole>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetOnlyActiveRole", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ActiveRole isActive = new ActiveRole
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            RoleName = reader["RoleName"].ToString(),
                        };
                        RoleList.Add(isActive);

                    }
                }
            }
            return RoleList;
        }
    }
}
