namespace ETicaret.UI.Areas.Admin.Models.AdminVMs
{
    public class AdminListVM
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[]? Image { get; set; }
    }
}
