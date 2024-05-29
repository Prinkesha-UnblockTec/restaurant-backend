namespace restaurant.Models
{
    public class LoginUserWithRole
    {
        public int ID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public int? RoleID { get; set; }
    }
}
