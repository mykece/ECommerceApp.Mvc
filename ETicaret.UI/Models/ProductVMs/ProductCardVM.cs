using ETicaret.Domain.Entities;
using ETicaret.UI.Areas.Admin.Models.CategorySizeTypeProductVMs;
using ETicaret.UI.Areas.Admin.Models.SizeVMs;

namespace ETicaret.UI.Models.ProductVMs
{
    public class ProductCardVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public byte[] Image { get; set; }
        public List<SizeListVM> Sizes { get; set; }
        public decimal? OriginalPrice { get; set; }

    }
}
