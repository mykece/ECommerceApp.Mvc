using ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs;

namespace ETicaret.UI.Areas.Admin.Models.ProductVMs
{
    public class CreateProductRequest
    {
        public ProductCreateVM ProductCreateVM { get; set; }
        public List<string> CategoryIds { get; set; }
        public List<string> SizeIds { get; set; }
        public List<CategorySizeTypeProductCreateDTO> CategorySizeTypeProductCreateDTOs { get; set; }
    }
}
