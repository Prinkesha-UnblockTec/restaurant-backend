namespace restaurant.Models
{
    public class OrderDetails
    {
        public string? ItemName { get; set; }
        public string? Month { get; set; }
        public string? OrderDate { get; set; } // Use string if the date is formatted as a string
        public int TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
    }
}
