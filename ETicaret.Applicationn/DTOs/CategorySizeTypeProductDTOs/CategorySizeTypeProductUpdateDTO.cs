using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs
{
    public class CategorySizeTypeProductUpdateDTO
    {
        public Guid? Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid CategorySizeTypeId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SizeId { get; set; }
        public double Quantity { get; set; }
    }
}
