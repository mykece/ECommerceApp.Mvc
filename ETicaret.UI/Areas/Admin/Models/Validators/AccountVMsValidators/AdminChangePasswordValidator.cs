using ETicaret.UI.Areas.Admin.Models.AccountVMs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Models.Validators.AccountVMsValidators
{
    public class AdminChangePasswordValidator : AbstractValidator<ChangePasswordVM>
    {
        public AdminChangePasswordValidator(IStringLocalizer<ModelResource> localizer)
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage(localizer["You must enter the current password!"])
                .NotNull().WithMessage(localizer["Password field cannot be left blank!"]); //NotEmpty kısmında string empty kabul etmiyor.
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage(localizer["You must enter your new password!"])
                .NotNull().WithMessage(localizer["The new password field cannot be left blank!"])
                .MinimumLength(6).WithMessage(localizer["Password must be at least 6 characters long!"]);

        }
    }
}
