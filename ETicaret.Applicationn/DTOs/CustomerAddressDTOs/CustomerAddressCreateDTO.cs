using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.CustomerAddressDTOs
{
    public class CustomerAddressCreateDTO
    {
        public string AddressName { get; set; }
        public string Address { get; set; }
        public Guid CustomerId { get; set; }
    }
}
