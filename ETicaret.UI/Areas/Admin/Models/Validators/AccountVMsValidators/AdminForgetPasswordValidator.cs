using ETicaret.UI.Areas.Admin.Models.AccountVMs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Models.Validators.AccountVMsValidators
{
    public class AdminForgetPasswordValidator : AbstractValidator<ForgotPasswordVM>
    {
        public AdminForgetPasswordValidator(IStringLocalizer<ModelResource> localizer)
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizer["Please enter an e-mail in a valid format!"])
                .NotEmpty().WithMessage(localizer["Fill in the e-mail field!"])
                .NotNull().WithMessage(localizer["Fill in the e-mail field!"]); //db'ye boş sorgu attırmamak için e-mail type kuralı koydum.
        }
    }
}
