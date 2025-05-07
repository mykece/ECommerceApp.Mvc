using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicaret.UI.Areas.Admin.Models.SizeVMs
{
    public class SizeCreateVM
    {
        public string SizeName { get; set; }
        public string? Description { get; set; }

        public SelectList? SizeTypes { get; set; }
        public Guid SizeTypeId { get; set; }
    }
}
