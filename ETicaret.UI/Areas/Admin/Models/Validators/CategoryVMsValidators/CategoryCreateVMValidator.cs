using ETicaret.UI.Areas.Admin.Models.CategoryVMs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Models.Validators.CategoryVMsValidators
{
    public class CategoryCreateVMValidator : AbstractValidator<CategoryCreateVM>
    {
        public CategoryCreateVMValidator(IStringLocalizer<ModelResource> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["The category name cannot be ignored!"])
                 .NotNull().WithMessage(localizer["The category name cannot be null!"]);
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizer["The description cannot be ignored!"])
                .NotNull().WithMessage(localizer["The description cannot be null!"]);
            RuleFor(x => x.NewImage)
                .NotEmpty().WithMessage(localizer["You must select an image."])
                .NotNull().WithMessage(localizer["The image cannot be null."]);
        }
    }
}
