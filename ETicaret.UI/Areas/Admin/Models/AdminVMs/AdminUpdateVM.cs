namespace ETicaret.UI.Areas.Admin.Models.AdminVMs
{
    public class AdminUpdateVM
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IFormFile? NewImage { get; set; }
        public byte[]? ExistingImage { get; set; }
    }
}
