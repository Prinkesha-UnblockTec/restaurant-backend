using restaurant.Interfaces;
using restaurant.Models;
using System.Data.SqlClient;
using System.Data;

namespace restaurant.Repository
{
    public class MenuAceessRepository : IMenuAceessRepository
    {
        private readonly IConfiguration _configuration;

        public MenuAceessRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ICollection<MenuAccess> GetAllMenuAccessData()
        {
            var MenuList = new List<MenuAccess>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllMenuAccessData", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MenuAccess Monus = new MenuAccess
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            RoleID = Convert.ToInt32(reader["RoleID"]),
                            MenuID = Convert.ToInt32(reader["MenuID"]),
                            MenuName = reader["MenuName"].ToString(),
                            CanAdd = Convert.ToInt32(reader["CanAdd"]),
                            CanEdit = Convert.ToInt32(reader["CanEdit"]),
                            CanDelete = Convert.ToInt32(reader["CanDelete"]),
                            CanView = Convert.ToInt32(reader["CanView"]),
                        };
                        MenuList.Add(Monus);

                    }
                }
            }
            return MenuList;
        }

        public ICollection<MenuAccess> GetMenuListByRoleID(int RoleId)
        {
            var menuList = new List<MenuAccess>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("GetMenuListByRoleID", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                cmd.Parameters.AddWithValue("@RoleID", RoleId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MenuAccess menu = new MenuAccess
                        {
                            MenuID = Convert.ToInt32(reader["MenuID"]),
                            MenuName = reader["MenuName"].ToString(),
                            RoleID = Convert.ToInt32(reader["RoleID"]),
                            CanAdd = Convert.ToInt32(reader["CanAdd"]),
                            CanEdit = Convert.ToInt32(reader["CanEdit"]),
                            CanDelete = Convert.ToInt32(reader["CanDelete"]),
                            CanView = Convert.ToInt32(reader["CanView"])
                        };
                        menuList.Add(menu);
                    }
                }
            }
            return menuList;
        }
        public bool SaveMenuAccessData(List<MenuAccess> model)
        {
            foreach (var item in model)
            {
                string? connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SaveMenuAccessData", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", item.ID);
                        cmd.Parameters.AddWithValue("@RoleID", item.RoleID);
                        cmd.Parameters.AddWithValue("@MenuID", item.MenuID);
                        cmd.Parameters.AddWithValue("@MenuName", item.MenuName);
                        cmd.Parameters.AddWithValue("@CanAdd", item.CanAdd);
                        cmd.Parameters.AddWithValue("@CanEdit", item.CanEdit);
                        cmd.Parameters.AddWithValue("@CanDelete", item.CanDelete);
                        cmd.Parameters.AddWithValue("@CanView", item.CanView);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            return true;
        }
        public ICollection<Menu> GetActiveMenu()
        {
            var MenuList = new List<Menu>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetOnlyMenu", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Menu Menus = new Menu
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            MenuName = reader["MenuName"].ToString(),
                        };
                        MenuList.Add(Menus);

                    }
                }
            }
            return MenuList;
        }
        public ICollection<ActiveMenuAccess> GetDatasBaseOnRoleID(int RoleId)
        {
            var MenuList = new List<ActiveMenuAccess>();
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("GetDataBaseOnRoleID", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                cmd.Parameters.AddWithValue("@RoleID", RoleId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ActiveMenuAccess MenuAccess = new ActiveMenuAccess
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            RoleID = Convert.ToInt32(reader["RoleID"]),
                            MenuName = reader["MenuName"].ToString(),
                            MenuID = Convert.ToInt32(reader["MenuID"]),
                            CanAdd = Convert.ToInt32(reader["CanAdd"]),
                            CanEdit = Convert.ToInt32(reader["CanEdit"]),
                            CanDelete = Convert.ToInt32(reader["CanDelete"]),
                            CanView = Convert.ToInt32(reader["CanView"]),
                        };
                        MenuList.Add(MenuAccess);
                    }
                }
            }
            return MenuList;
        }
    }
}
