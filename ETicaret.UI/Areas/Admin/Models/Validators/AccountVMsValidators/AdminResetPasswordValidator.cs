using ETicaret.UI.Areas.Admin.Models.AccountVMs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Models.Validators.AccountVMsValidators
{
    public class AdminResetPasswordValidator: AbstractValidator<ResetPasswordVM>
    {
        public AdminResetPasswordValidator(IStringLocalizer<ModelResource> localizer)
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage(localizer["Password field cannot be left blank!"])
                .NotNull().WithMessage(localizer["Password field cannot be left blank!"]);
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizer["Please enter an email address in valid format."]);
        }
    }
}



