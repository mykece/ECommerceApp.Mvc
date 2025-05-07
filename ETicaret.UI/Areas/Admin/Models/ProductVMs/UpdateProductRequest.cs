using ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs;

namespace ETicaret.UI.Areas.Admin.Models.ProductVMs
{
    public class UpdateProductRequest
    {
   
        public ProductUpdateVM ProductUpdateVM { get; set; }
        public List<string> CategoryIds { get; set; }
        public List<string> SizeIds { get; set; }
        public List<CategorySizeTypeProductUpdateDTO> CategorySizeTypeProductUpdateDTOs { get; set; }
    }
}
