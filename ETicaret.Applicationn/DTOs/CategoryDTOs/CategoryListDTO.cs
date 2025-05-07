using ETicaret.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.CategoryDTOs
{
    public class CategoryListDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public Guid SizeTypeId { get; set; }
        public byte[]? Image { get; set; }
        public List<Guid> SizeTypeIds { get; set; } = new List<Guid>();
        public List<string> SizeTypeNames { get; set; } = new List<string>();
        public int ProductCount { get; set; }
        public Gender Gender { get; set; }
    }
}
