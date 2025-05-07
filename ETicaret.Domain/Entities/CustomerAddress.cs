using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Domain.Entities
{
    public class CustomerAddress : BaseEntity
    {
        public string AddressName { get; set; }
        public string Address { get; set; }

        public virtual Customer Customer { get; set; }
        public Guid CustomerId { get; set; }
    }
}
