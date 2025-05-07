using ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs;
using ETicaret.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicaret.UI.Areas.Admin.Models.ProductVMs
{
    public class ProductCreateVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public double Quantity { get; set; }
        public IFormFile NewImage { get; set; }
        public SelectList? Categories { get; set; }
        public List<Guid> CategoryIds { get; set; }
        public Guid CategoryId { get; set; }
        public SelectList? Sizes { get; set; }
        public Guid SizeId { get; set; }
        public List<CategorySizeTypeProductCreateDTO> CategorySizeTypeProductCreateDTOs { get; set; }
        public Gender Gender { get; set; }

    }
}
