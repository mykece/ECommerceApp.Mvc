using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.CategorySizeTypeDTOs
{
    public class CategorySizeTypeUpdateDTO
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SizeTypeId { get; set; }
    }
}
