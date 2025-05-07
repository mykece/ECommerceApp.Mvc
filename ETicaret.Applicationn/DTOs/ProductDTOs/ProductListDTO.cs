using ETicaret.Domain.Enums;

namespace ETicaret.Applicationn.DTOs.ProductDTOs
{
    public class ProductListDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public byte[]? Image { get; set; }
        public decimal? OriginalPrice { get; set; }
        public double Quantity { get; set; }
        public List<ProductQuantityDetailDTO> QuantityDetails { get; set; } // Beden bazlı quantity bilgileri
        public List<string> CategoryNames { get; set; }
        public Gender Gender { get; set; }
    }

    //public class ProductQuantityDetailDTO
    //{
    //    public string SizeName { get; set; } // Beden adı (örneğin, S, M, L)
    //    public double Quantity { get; set; } // Beden adedini tutan property
    //}

}
