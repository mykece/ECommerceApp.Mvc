using ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs;
using ETicaret.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicaret.Applicationn.DTOs.ProductDTOs
{
    public class ProductUpdateDTO
    {

        //public string Name { get; set; }
        //public string Description { get; set; }
        //public decimal UnitPrice { get; set; }
        //public Guid CategorySizeTypeId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public double Quantity { get; set; }
        public byte[]? Image { get; set; }
        public List<Guid> CategoryIds { get; set; }
        public List<Guid> SizeIds { get; set; }

        public List<CategorySizeTypeProductUpdateDTO> CategorySizeTypeProductUpdateDTOs { get; set; }
        public Gender Gender { get; set; }

    }
}
