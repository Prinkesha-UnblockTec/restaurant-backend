namespace restaurant.Models
{
    public class StausWiseShowOrder
    {
        public ICollection<ShowOrders> ShowOrderss { get; set; }
        public int TotalRecords { get; set; }
    }
}
