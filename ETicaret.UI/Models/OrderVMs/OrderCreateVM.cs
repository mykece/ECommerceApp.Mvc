﻿namespace ETicaret.UI.Models.OrderVMs
{
    public class OrderCreateVM
    {
        public string ProductName { get; set; }
        public string UnitPrice { get; set; }
        public string Quantity { get; set; }
        public string SizeName { get; set; }
        public string SizeId { get; set; }
        public string IdentityId { get; set; }
    }
}
