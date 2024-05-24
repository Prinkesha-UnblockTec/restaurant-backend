using Microsoft.Extensions.Configuration;
using restaurant.Interfaces;
using restaurant.Models;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace restaurant.Repository
{
    public class CategoriesItemsRepository : ICategoriesItemsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment environment;

        public CategoriesItemsRepository(IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.environment = environment;
            _configuration = configuration;
        }
        public bool AddCategoriesItemsList(CategoriesItems model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddCategoriesItemsData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemName", model.ItemName);
                    cmd.Parameters.AddWithValue("@Price", model.Price);
                    cmd.Parameters.AddWithValue("@CategoriesName", model.CategoriesName);
                    cmd.Parameters.AddWithValue("@Description", model.Description);
                    cmd.Parameters.AddWithValue("@BalanceQuantity", model.BalanceQuantity);
                    int commaIndex = model.ImageBase64.IndexOf(',');
                    if (commaIndex >= 0)
                    {
                        model.ImageBase64 = model.ImageBase64.Substring(commaIndex + 1);
                    }
                    string imagePath = SaveBase64Image(model.ImageBase64);
                    cmd.Parameters.AddWithValue("@ImageURL", imagePath);
                    cmd.Parameters.AddWithValue("@Status", model.Status);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
     
        private string GetFilepath(string productcode)
        {
            return this.environment.WebRootPath + "\\Images\\Items\\" + productcode;
        }
        public bool DeleteCategoriesItem(int id)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteCategoriesItem", con)
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

        public bool EditCategoriesItemsList(CategoriesItems model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EditCategoriesItemsData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@ItemName", model.ItemName);
                    cmd.Parameters.AddWithValue("@Price", model.Price);
                    cmd.Parameters.AddWithValue("@BalanceQuantity", model.BalanceQuantity);
                    cmd.Parameters.AddWithValue("@CategoriesName", model.CategoriesName);
                    cmd.Parameters.AddWithValue("@Description", model.Description);
                    if (model.ImageBase64 != null)
                    {
                        int commaIndex = model.ImageBase64.IndexOf(',');
                        if (commaIndex >= 0)
                        {
                            model.ImageBase64 = model.ImageBase64.Substring(commaIndex + 1);
                        }
                        string imagePath = SaveBase64Image(model.ImageBase64);
                        cmd.Parameters.AddWithValue("@ImageURL", imagePath);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ImageURL", model.ImageURL);
                    }
                    cmd.Parameters.AddWithValue("@Status", model.Status);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        private string SaveBase64Image(string base64)
        {
            string fileName = $"{Guid.NewGuid().ToString()}.jpg";
            string FilePath = GetFilepath(fileName);
            byte[] imageBytes = Convert.FromBase64String(base64);
            File.WriteAllBytes(FilePath, imageBytes);
            return "Images\\Items\\" + fileName;
        }

        public ICollection<ActiveCategoriesItems> GetAllActiveCategoriesItemsData()
        {
            var ActiveCategoriesList = new List<ActiveCategoriesItems>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetOnlyActiveCategoriesItems", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ActiveCategoriesItems isActive = new ActiveCategoriesItems
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            BalanceQuantity = Convert.ToInt32(reader["BalanceQuantity"]),
                            ItemName = reader["ItemName"].ToString(),
                            CategoriesName = reader["CategoriesName"].ToString(),
                            Description = reader["Description"].ToString(),
                            ImageURL = reader["ImageURL"].ToString(),
                            Status = reader["Status"].ToString(),
                            Calculation = reader["Calculation"] != DBNull.Value ? reader["Calculation"].ToString() : string.Empty,
                            OriginalQuantity = reader["BalanceQuantity"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("BalanceQuantity")) : 0,

                        };
                        ActiveCategoriesList.Add(isActive);

                    }
                }
                return ActiveCategoriesList;
            }
        }

        public ICollection<CategoriesItems> GetAllCategoriesItemsData()
        {
            var CategoriesList = new List<CategoriesItems>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllCategoriesItemsData", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CategoriesItems item = new CategoriesItems
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            BalanceQuantity = reader["BalanceQuantity"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("BalanceQuantity")) : 0,
                            ItemName = reader["ItemName"].ToString(),
                            CategoriesName = reader["CategoriesName"].ToString(),
                            Description = reader["Description"].ToString(),
                            ImageURL = reader["ImageURL"].ToString(),
                            Status = reader["Status"].ToString(),
                            Calculation = reader["Calculation"] != DBNull.Value ? reader["Calculation"].ToString() : string.Empty,
                            OriginalQuantity = reader["BalanceQuantity"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("BalanceQuantity")) : 0,

                        };
                        CategoriesList.Add(item);

                    }
                }
            }
            return CategoriesList;
        }

        public bool EditUpdateBalanceQuantityList(List<UpdateBalanceQuantity> ItemsArray)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                foreach (var product in ItemsArray)
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateBalanceQuantity", con))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ItemId", product.ItemId);
                            cmd.Parameters.AddWithValue("@NewStock", product.NewStock);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            return true;
        }

        public bool UpdateCalculationItems(UpdateCalculation model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateCalculationItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.Id);
                    cmd.Parameters.AddWithValue("@Calculation", model.Calculation);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
    }
}
