﻿namespace restaurant.Models
{
    public class UserCartFinalDetails
    {
        public class Products
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

        public class CartDetails
        {
            public int? ID { get; set; }
            public int? LoginId { get; set; }
            public int? PinCode { get; set; }
            public string? Username { get; set; }
            public string? TableNo { get; set; }
            public string? OrderType { get; set; }
            public string? Date { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public string? Address { get; set; }
            public string? DeliverName { get; set; }
            public string? Currency { get; set; }
            public string? Time { get; set; }
            public string? PaymentType { get; set; }
            public string? UPIId { get; set; }
            public string? CardNumber { get; set; }
            public string? CardName { get; set; }
            public string? ExpireDate { get; set; }
            public string? BankName { get; set; }
            public List<Products> Products { get; set; } = new List<Products>();
        }
    }
}
