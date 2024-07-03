namespace restaurant.Models
{
    public class ChefMaster
    {
        public int ID { get; set; }
        public string? ChefName { get; set; }
        public List<string> ItemList { get; set; }
        public string? Status { get; set; }
    }
}
