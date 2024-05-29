using System.Reflection.PortableExecutable;

namespace restaurant.Models
{
    public class User
    {
        public int ID { get; set; }
        public string? UserName { get; set; }
        public int RoleID { get; set; }
        public string? Role { get; set; }
        public string? Password { get; set; }
        public string? Status { get; set; }
    }
}
