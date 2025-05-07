using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.ProductDTOs
{
    public class ProductQuantityDetailDTO
    {
        public string SizeName { get; set; } // Beden adı (örneğin, S, M, L)
        public double Quantity { get; set; } // Beden adedini tutan property
    }
}
