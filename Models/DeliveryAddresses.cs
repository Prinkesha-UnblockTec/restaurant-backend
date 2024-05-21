namespace restaurant.Models
{
    public class DeliveryAddresses
    {
        public int? ID { get; set; }
        public int? LoginId { get; set; }
        public int? PinCode { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? DeliveryName { get; set; }
        public int? isDefult { get; set; }
    }
}
