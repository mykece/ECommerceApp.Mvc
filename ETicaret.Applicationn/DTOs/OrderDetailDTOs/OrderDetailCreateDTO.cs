using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.OrderDetailDTOs;

public class OrderDetailCreateDTO
{
    public double Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public double Discount { get; set; }
    public Guid CategorySizeTypeProductId { get; set; }
    public Guid? OrderId { get; set; }
}
