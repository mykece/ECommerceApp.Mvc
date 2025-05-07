using ETicaret.Applicationn.DTOs.ProductDTOs;
using ETicaret.Domain.Enums;

namespace ETicaret.UI.Areas.Admin.Models.ProductVMs
{
    public class ProductListVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? OriginalPrice { get; set; }
        public byte[] Image { get; set; }
        public Guid SizeId { get; set; }
        public List<ProductQuantityDetailDTO> QuantityDetails { get; set; } // Beden bazlı quantity bilgileri
        public bool IsAuthenticated { get; set; }
        public List<string> CategoryNames { get; set; }
        public Gender Gender { get; set; }
    }
}
