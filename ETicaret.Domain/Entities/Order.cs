using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Domain.Entities;

public class Order : AuditableEntity
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string ShipAdress { get; set; }
    public DateTime? ShippedDate { get; set; }
    public string? ShipVia { get; set; }
    public string ShipCity { get; set; }
    public string ShipCountry { get; set; }
    public int ShipPostalCode { get; set; }

    //Nav Prop

    public virtual List<OrderDetail> OrderDetails { get; set; }
    public virtual Customer Customer { get; set; }
    public Guid CustomerId { get; set; }
}
