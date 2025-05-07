using ETicaret.UI.Areas.Admin.Models.SizeVMs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Models.Validators.SizeVMsValidators
{
    public class SizeCreateVMValidator : AbstractValidator<SizeCreateVM>
    {
        public SizeCreateVMValidator(IStringLocalizer<ModelResource> localizer)
        {
            RuleFor(x => x.SizeName).NotEmpty().WithMessage(localizer["The size name cannot be ignored!"])
           .NotNull().WithMessage(localizer["The size name cannot be null!"]);
            RuleFor(x => x.Description)
           .MaximumLength(200).WithMessage(localizer["Description cannot exceed 500 characters"]);


        }
    }
}
