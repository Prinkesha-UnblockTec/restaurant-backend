using restaurant.Interfaces;
using restaurant.Models;
using System.Data.SqlClient;
using System.Data;

namespace restaurant.Repository
{
    public class MasterRepository : IMasterRepository
    {
        private readonly IConfiguration _configuration;

        public MasterRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool AddedMasterList(Master model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddMasterData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TableNo", model.TableNo);
                    cmd.Parameters.AddWithValue("@Status", model.Status);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public bool DeleteMaster(int id)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteMaster", con)
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

        public bool EditMaster(Master model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EditMasterData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@TableNo", model.TableNo);
                    cmd.Parameters.AddWithValue("@Status", model.Status);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public ICollection<ActiveMaster> GetActiveMaster()
        {
            var ActiveMasterList = new List<ActiveMaster>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetOnlyActiveMaster", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ActiveMaster isActive = new ActiveMaster
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            TableNo = reader["TableNo"].ToString(),
                        };
                        ActiveMasterList.Add(isActive);

                    }
                }
            }
            return ActiveMasterList;
        }

        public ICollection<Master> GetAllMasterData()
        {
            var MasterList = new List<Master>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllMasterData", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Master item = new Master
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            TableNo = reader["TableNo"].ToString(),
                            Status = reader["Status"].ToString(),
                        };
                        MasterList.Add(item);

                    }
                }
            }
            return MasterList;
        }

        public ICollection<CartIDAndUserName> GetLastRecordByTableNo(string TableNo)
        {
            var CartIDAndUserNameList = new List<CartIDAndUserName>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetLastRecordByTableNo", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@TableNo", TableNo));

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CartIDAndUserName item = new CartIDAndUserName
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            UserName = reader["UserName"].ToString(),
                        };
                        CartIDAndUserNameList.Add(item);

                    }
                }
            }
            return CartIDAndUserNameList;
        }

        public ICollection<TableNoBaseTotalAmount> GetTotalAmountTableNo()
        {
            var TotalAmountList = new List<TableNoBaseTotalAmount>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetTotalPriceByTableNo", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TableNoBaseTotalAmount item = new TableNoBaseTotalAmount
                        {
                            TotalAmount = Convert.ToInt32(reader["TotalAmount"]),
                            TableNo = reader["TableNo"].ToString(),
                        };
                        TotalAmountList.Add(item);

                    }
                }
            }
            return TotalAmountList;
        }
    }
}
