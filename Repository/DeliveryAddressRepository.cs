using restaurant.Interfaces;
using restaurant.Models;
using System.Data.SqlClient;
using System.Data;

namespace restaurant.Repository
{
    public class DeliveryAddressRepository : IDeliveryAddressRepository
    {
        private readonly IConfiguration _configuration;

        public DeliveryAddressRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool AddedDeliveryAddressesList(DeliveryAddresses model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("addedDeliveryAddress", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LoginId", model.LoginId);
                    cmd.Parameters.AddWithValue("@PinCode", model.PinCode);
                    cmd.Parameters.AddWithValue("@Address", model.Address);
                    cmd.Parameters.AddWithValue("@City", model.City);
                    cmd.Parameters.AddWithValue("@State", model.State);
                    cmd.Parameters.AddWithValue("@DeliveryName", model.DeliveryName);
                    cmd.Parameters.AddWithValue("@isDefult", model.isDefult);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public bool DeleteDeliveryAddresses(int id)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteDeliveryAddress", con)
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

        public bool EditDeliveryAddresses(DeliveryAddresses model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EditDeliveryAddress", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@LoginId", model.LoginId);
                    cmd.Parameters.AddWithValue("@PinCode", model.PinCode);
                    cmd.Parameters.AddWithValue("@Address", model.Address);
                    cmd.Parameters.AddWithValue("@City", model.City);
                    cmd.Parameters.AddWithValue("@State", model.State);
                    cmd.Parameters.AddWithValue("@DeliveryName", model.DeliveryName);
                    cmd.Parameters.AddWithValue("@isDefult", model.isDefult);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public ICollection<DeliveryAddresses> GetAllDeliveryAddressesData(int loginId)
        {
            var DeliveryAddressesList = new List<DeliveryAddresses>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDeliveryAddress", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@LoginId", loginId));
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DeliveryAddresses items = new DeliveryAddresses
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            isDefult = Convert.ToInt32(reader["isDefult"]),
                            LoginId = Convert.ToInt32(reader["LoginId"]),
                            PinCode = Convert.ToInt32(reader["PinCode"]),
                            Address = reader["Address"].ToString(),
                            City = reader["City"].ToString(),
                            State = reader["State"].ToString(),
                            DeliveryName = reader["DeliveryName"].ToString(),
                        };
                        DeliveryAddressesList.Add(items);

                    }
                }
            }
            return DeliveryAddressesList;
        }
        public bool SetDefaultAddress(SetDefult model)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SetDefaultAddress", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@AddressId", model.AddressId));
                cmd.Parameters.Add(new SqlParameter("@LoginId", model.LoginId));
                con.Open();

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
        public ICollection<DeliveryAddresses> GetDefaultAddress(int loginId)
        {
            var DeliveryAddressesList = new List<DeliveryAddresses>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetDefaultAddress", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add(new SqlParameter("@LoginId", loginId));
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DeliveryAddresses items = new DeliveryAddresses
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            isDefult = Convert.ToInt32(reader["isDefult"]),
                            LoginId = Convert.ToInt32(reader["LoginId"]),
                            PinCode = Convert.ToInt32(reader["PinCode"]),
                            Address = reader["Address"].ToString(),
                            City = reader["City"].ToString(),
                            State = reader["State"].ToString(),
                            DeliveryName = reader["DeliveryName"].ToString(),
                        };
                        DeliveryAddressesList.Add(items);

                    }
                }
            }
            return DeliveryAddressesList;
        }

    }
}
