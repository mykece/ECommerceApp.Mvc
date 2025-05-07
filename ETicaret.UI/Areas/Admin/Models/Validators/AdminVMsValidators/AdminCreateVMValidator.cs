using ETicaret.UI.Areas.Admin.Models.AdminVMs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Models.Validators.AdminVMsValidators
{
    public class AdminCreateVMValidator : AbstractValidator<AdminCreateVM>
    {
        public AdminCreateVMValidator(IStringLocalizer<ModelResource> localizer)
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizer["Please enter data in e-mail format!"])
                 .NotEmpty().WithMessage(localizer["The e-mail field cannot be ignored!"])
                 .NotNull().WithMessage(localizer["The e-mail field cannot be null!"]);
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(localizer["The First Name field cannot be ignored!"])
                .NotNull().WithMessage(localizer["The First Name field cannot be null!"]);
            RuleFor(x => x.LastName).NotEmpty().WithMessage(localizer["The Last Name field cannot be ignored!"])
                .NotNull().WithMessage(localizer["The Last Name field cannot be null!"]);


        }
    }
}
