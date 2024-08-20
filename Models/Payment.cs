namespace restaurant.Models
{
    public class Payment
    {
        public int Id { get; set; } 
        public int OrderID { get; set; }
        public string? PaymentType { get; set; }
        public string? UPIId { get; set; }
        public string? CardNumber { get; set; }
        public string? CardName { get; set; }
        public string? ExpireDate { get; set; }
        public string? BankName { get; set; }
    }
}
