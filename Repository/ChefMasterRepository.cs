using restaurant.Interfaces;
using restaurant.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace restaurant.Repository
{
    public class ChefMasterRepository : IChefMaster
    {
        private readonly IConfiguration _configuration;

        public ChefMasterRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool AddedChefMasterList(ChefMaster model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddChefData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChefName", model.ChefName);
                    cmd.Parameters.AddWithValue("@Status", model.Status);
                    cmd.Parameters.AddWithValue("@ItemList", string.Join(",", model.ItemList));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public bool DeleteChefMaster(int id)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteChefMaster", con)
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

        public bool EditChefMaster(ChefMaster model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EditChefData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", model.ID);
                    cmd.Parameters.AddWithValue("@ChefName", model.ChefName);
                    cmd.Parameters.AddWithValue("@Status", model.Status);
                    cmd.Parameters.AddWithValue("@ItemList", string.Join(",", model.ItemList));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }


        public ICollection<ActiveChef> GetActiveChefMaster()
        {
            throw new NotImplementedException();
        }

        public ICollection<ChefMaster> GetAllChefMasterData()
        {
            var ChefList = new List<ChefMaster>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllChefs", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ChefMaster chef = new ChefMaster
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            ChefName = reader["ChefName"].ToString(),
                            ItemList = reader["Items"].ToString().Split(',').ToList(),
                            Status = reader["Status"].ToString()
                        };
                        ChefList.Add(chef);

                    }
                }
            }
            return ChefList;
        }
      
    }
}
