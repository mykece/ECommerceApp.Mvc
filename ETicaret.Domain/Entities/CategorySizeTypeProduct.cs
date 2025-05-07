using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Domain.Entities
{
    public class CategorySizeTypeProduct : AuditableEntity
    {
        public double Quantity { get; set; }
        public Guid SizeId { get; set; }

        //Nav Prop
        public Guid CategorySizeTypeId { get; set; }
        public virtual CategorySizeType CategorySizeType { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }

    }
}
