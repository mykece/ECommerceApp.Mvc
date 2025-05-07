using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Domain.Entities
{
    public class Customer : BaseUser
    {
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }

        public virtual List<CustomerAddress>? CustomerAddresses { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}
