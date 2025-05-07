using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicaret.UI.Areas.Admin.Models.CategoryVMs
{
    public class CategoryUpdateVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public SelectList? SizeTypes { get; set; }
        //public Guid SizeTypeId { get; set; }
        public IFormFile? NewImage { get; set; }
        public byte[]? ExistingImage { get; set; }


        //Sonradan eklendi Multi select için
        public List<Guid> SizeTypeIds { get; set; } = new List<Guid>();
        public List<SelectListItem> SizeTypes { get; set; }
    }
}
