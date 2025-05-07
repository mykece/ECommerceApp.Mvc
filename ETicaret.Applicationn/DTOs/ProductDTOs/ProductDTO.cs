using ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs;
using ETicaret.Domain.Enums;

namespace ETicaret.Applicationn.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public byte[]? Image { get; set; }
        public List<Guid> CategoryIds { get; set; }
        public List<Guid> SizeIds { get; set; }
        public double Quantity { get; set; }
        public List<CategorySizeTypeProductCreateDTO> CategorySizeTypeProductCreateDTOs { get; set; }
        public List<ProductQuantityDetailDTO> QuantityDetails { get; set; } // Yeni eklenen özellik
        public Gender Gender { get; set; }
        public decimal? OriginalPrice { get; set; }
    }
}
