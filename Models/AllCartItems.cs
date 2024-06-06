namespace restaurant.Models
{
    public class AllCartItems
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string? Status { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? Currency { get; set; }
        public string? OrderType { get; set; }
    }
}
