using ETicaret.UI.Areas.Admin.Models.SizeTypesVMs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Models.Validators.SizeTypeVMsValidators
{
    public class SizeTypeCreateVMValidator : AbstractValidator<SizeTypeCreateVM>
    {
        public SizeTypeCreateVMValidator(IStringLocalizer<ModelResource> localizer)
        {
            RuleFor(x => x.SizeTypeName).NotEmpty().WithMessage(localizer["The sizetype name cannot be ignored!"])
            .NotNull().WithMessage(localizer["The sizetype name cannot be null!"]);
        }
    }
}
