using ETicaret.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs
{
    public class CategorySizeTypeProductCreateDTO
    {
        public Guid ProductId { get; set; }
        public Guid CategorySizeTypeId { get; set; }
        public Guid SizeId { get; set; }
        public double Quantity { get; set; }

    }
}
