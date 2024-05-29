namespace restaurant.Models
{
    public class Items
    {
        public int Id { get; set; }
        public int LoginId { get; set; }
        public string? Date { get; set; }
        public int Price { get; set; }
        public string? ImageURL { get; set; }
        public int Quantity { get; set; }
        public string? UserName { get; set; }
        public string? Time { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
    }
}
