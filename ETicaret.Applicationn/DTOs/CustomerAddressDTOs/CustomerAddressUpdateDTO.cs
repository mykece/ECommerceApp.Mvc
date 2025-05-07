using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.CustomerAddressDTOs
{
    public class CustomerAddressUpdateDTO
    {
        public Guid Id { get; set; }
        public string AddressName { get; set; }
        public string Address { get; set; }
        public Guid CustomerId { get; set; }
    }
}
