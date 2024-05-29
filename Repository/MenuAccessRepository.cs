using restaurant.Interfaces;
using restaurant.Models;
using System.Data.SqlClient;
using System.Data;

namespace restaurant.Repository
{
    public class MenuAccessRepository : IMenuAccessRepository
    {
        private readonly IConfiguration _configuration;

        public MenuAccessRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool GetActiveMenu(Menu menu)
        {
            throw new NotImplementedException();
        }
        public bool EditMenuAccessDatas(MonuAccess model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("MenuAccessDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.Id);
                    cmd.Parameters.AddWithValue("@IsShow", model.IsShow);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;

        }
        public ICollection<MonuAccess> GetAllMenuAccessData()
        {
            var RoleList = new List<MonuAccess>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetMenuAccessDetails", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MonuAccess Roles = new MonuAccess
                        {
                            Id = Convert.ToInt32(reader["ID"]),
                            MenuName = reader["MenuName"].ToString(),
                            IsShow = reader["IsShow"].ToString(),
                        };
                        RoleList.Add(Roles);

                    }
                }
            }
            return RoleList;
        }
    }
}
