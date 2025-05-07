using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Domain.Entities;

public class OrderDetail : BaseEntity
{
    public double Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public double Discount { get; set; }


    //Nav Prop
    public virtual CategorySizeTypeProduct CategorySizeTypeProduct { get; set; }
    public  Guid CategorySizeTypeProductId { get; set; }
    public virtual Order Order { get; set; }
    public  Guid OrderId { get; set; }
}
