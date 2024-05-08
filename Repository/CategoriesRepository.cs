using restaurant.Interfaces;
using restaurant.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.IO;

namespace restaurant.Repository
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment environment;

        public CategoriesRepository(IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.environment = environment;
            _configuration = configuration;
        }
        public ICollection<ActiveCategories> GetAllActiveCategoriesData()
        {
            var ActiveCategoriesList = new List<ActiveCategories>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetOnlyActiveCategories", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ActiveCategories isActive = new ActiveCategories
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            CategoriesName = reader["CategoriesName"].ToString(),
                            ImageURL = reader["ImageURL"].ToString(),
                        };
                        ActiveCategoriesList.Add(isActive);

                    }
                }
                return ActiveCategoriesList;
            }
        }

        public ICollection<Categories> GetAllCategoriesData()
        {
            var CategoriesList = new List<Categories>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllCategoriesData", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Categories item = new Categories
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            CategoriesName = reader["CategoriesName"].ToString(),
                            ImageURL = reader["ImageURL"].ToString(),
                            Status = reader["Status"].ToString(),
                        };
                        CategoriesList.Add(item);

                    }
                }
            }
            return CategoriesList;
        }
        public bool AddCategoriesList(NewCategories model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddCategoriesData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CategoriesName", model.CategoriesName);
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
        public bool EditCategoriesList(NewCategories model)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EditCategoriesData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@CategoriesName", model.CategoriesName);
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
        //private string SaveBase64Image(string base64)
        //{
        //    string fileName = $"{Guid.NewGuid().ToString()}.jpg";
        //    string FilePath = GetFilepath(fileName);
        //    byte[] imageBytes = Convert.FromBase64String(base64);
        //    File.WriteAllBytes(FilePath, imageBytes);
        //    return "Images\\Categories\\" + fileName;
        //}
        private string SaveBase64Image(string base64)
        {
            string fileName = $"{Guid.NewGuid().ToString()}.jpg";
            string filePath = GetFilepath(fileName);

            byte[] imageBytes = Convert.FromBase64String(base64);

            // Resize the image
            using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(imageBytes))
            {
                int targetWidth = 300;
                int targetHeight = (int)((float)image.Height / image.Width * targetWidth);

                image.Mutate(x => x.Resize(targetWidth, targetHeight));

                // Save the resized image as JPEG with quality setting
                using (FileStream output = File.Create(filePath))
                {
                    image.Save(output, new JpegEncoder { Quality = 90 });
                }
            }

            return "Images\\Categories\\" + fileName;
        }

        private string GetFilepath(string productcode)
        {
            return this.environment.WebRootPath + "\\Images\\Categories\\" + productcode;
        }
        public bool DeleteCategories(int id)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteCategories", con)
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
