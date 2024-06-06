namespace restaurant.Models
{
    public class CartModel
    {
        public int CartId { get; set; }
        public string? UserCartId { get; set; }
        public string? TableNo { get; set; }
        public string? Date { get; set; }
        public string? Currency { get; set; }
        public List<AddedDataBaseOnCartId> Products { get; set; }
    }
}
