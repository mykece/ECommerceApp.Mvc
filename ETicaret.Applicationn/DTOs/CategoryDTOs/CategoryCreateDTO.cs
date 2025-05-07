using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.CategoryDTOs
{
    public class CategoryCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //public Guid SizeTypeId { get; set; }
        public byte[] Image { get; set; }
        public List<Guid> SizeTypeIds { get; set; } = new List<Guid>();
    }
}
