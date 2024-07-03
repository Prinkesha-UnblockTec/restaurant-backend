namespace restaurant.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Status { get; set; }
        public int IsRead { get; set; }
        public string? Description { get; set; }
        public string? RoleName { get; set; }
        public string? OrderType { get; set; }
        public DateTime DateTime { get; set; }
        
    }
}
