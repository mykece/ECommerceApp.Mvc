using ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs;
using ETicaret.Domain.Enums;

namespace ETicaret.Applicationn.DTOs.ProductDTOs
{
    public class ProductCreateDTO
    {
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal UnitPrice { get; set; }
        public byte[]? Image { get; set; }
        public List<Guid> CategoryIds { get; set; }
		public List<CategorySizeTypeProductCreateDTO> CategorySizeTypeProductCreateDTOs { get; set; }
        public Gender Gender { get; set; }
    }
}
