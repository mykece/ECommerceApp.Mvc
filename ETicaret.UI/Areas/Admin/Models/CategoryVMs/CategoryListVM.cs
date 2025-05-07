using ETicaret.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicaret.UI.Areas.Admin.Models.CategoryVMs
{
    public class CategoryListVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public Guid SizeTypeId { get; set; }
        public byte[] Image { get; set; }
        public List<string> SizeTypeNames { get; set; } = new List<string>();
        public int ProductCount { get; set; }
        public Gender Gender { get; set; }
    }
}
