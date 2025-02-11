﻿namespace restaurant.Models
{
    public class CategoriesItems
    {
        public int ID { get; set; }
        public string? ItemName { get; set; }
        public int Price { get; set; }
        public int? BalanceQuantity { get; set; }
        public int? OriginalQuantity { get; set; }
        public string? CategoriesName { get; set; }
        public string? Calculation { get; set; }
        public string?   Description { get; set; }
        public string? ImageBase64 { get; set; }
        public string? ImageURL { get; set; }
        public string? Status { get; set; }
    }
}
