using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicaret.UI.Areas.Admin.Models.CustomerVMs
{
    public class CustomerAddressListVM
    {
        public Guid Id { get; set; }
        public string AddressName { get; set; }
        public string Address { get; set; }
        public Guid CustomerId { get; set; }
    }
}
