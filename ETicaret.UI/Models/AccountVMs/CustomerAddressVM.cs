namespace ETicaret.UI.Models.AccountVMs
{
    public class CustomerAddressVM
    {
        public Guid Id { get; set; }
        public string AddressName { get; set; }
        public string Address { get; set; }
        //public DateTime AddressDate { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
