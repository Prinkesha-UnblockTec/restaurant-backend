namespace restaurant.Models
{
    public class OrdersAdmin
    {
        public int Id { get; set; }
        public bool Checked { get; set; }
        public string? UserName { get; set; }
        public string? ChefName { get; set; }
        public int Quantity { get; set; }
        public string? ImageURL { get; set; }
        public string? CategoriesName { get; set; }
        public string? Description { get; set; }
        public string? Currency { get; set; }
        public int Price { get; set; }
        public string? Status { get; set; }
        public string? Date { get; set; }
        public string? Password { get; set; }
        public string? ItemName { get; set; }
    }
}
