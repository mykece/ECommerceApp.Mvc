

namespace ETicaret.UI.Areas.Admin.Models.AccountVMs
{
    public class ChangePasswordVM
    {
        public Guid Id { get; set; }
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
