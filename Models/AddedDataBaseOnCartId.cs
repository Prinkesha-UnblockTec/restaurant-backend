namespace restaurant.Models
{
    public class AddedDataBaseOnCartId
    {
        public int? Id { get; set; }
        public int? LoginId { get; set; }
        public string? ItemName { get; set; }
        public int? Price { get; set; }
        public string? CategoriesName { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public int? Quantity { get; set; }
        public bool? IsSelected { get; set; }
    }
}
