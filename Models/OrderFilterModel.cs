namespace restaurant.Models
{
    public class OrderFilterModel
    {
        public List<string> ItemList { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}
