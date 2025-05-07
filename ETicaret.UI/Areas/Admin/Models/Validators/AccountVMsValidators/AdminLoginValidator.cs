using ETicaret.UI.Areas.Admin.Models.AccountVMs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Models.Validators.AccountVMsValidators
{
    public class AdminLoginValidator : AbstractValidator<LoginVM>
    {
        public AdminLoginValidator(IStringLocalizer<ModelResource> localizer)
        {
            RuleFor(x => x.Email).NotNull().WithMessage(localizer["The e-mail field cannot be left blank."])
                                  .NotEmpty().WithMessage(localizer["The e-mail field cannot be left blank."]);
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizer["Please enter an email address in valid format."]);
            RuleFor(x => x.Password).NotEmpty().WithMessage(localizer["Password field cannot be left blank."])
                                    .NotNull().WithMessage(localizer["Password field cannot be left blank."])
                                    .MinimumLength(6).WithMessage(localizer["Please enter a valid password!"]);
        }
    }
}
