namespace restaurant.Models
{
    public class TopSellingItemsParameters
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? TopOrBottom { get; set; }
    }
}
