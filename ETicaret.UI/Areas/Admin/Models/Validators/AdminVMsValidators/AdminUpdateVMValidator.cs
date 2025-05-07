using ETicaret.UI.Areas.Admin.Models.AdminVMs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Models.Validators.AdminVMsValidators
{
    public class AdminUpdateVMValidator : AbstractValidator<AdminUpdateVM>
    {
        public AdminUpdateVMValidator(IStringLocalizer<ModelResource> localizer)
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizer["Please enter data in e-mail format!"])
                                .NotNull().WithMessage(localizer["The e-mail field cannot be null!"])
                                .NotEmpty().WithMessage(localizer["The e-mail field cannot be ignored!"]);


        }
    }
}
