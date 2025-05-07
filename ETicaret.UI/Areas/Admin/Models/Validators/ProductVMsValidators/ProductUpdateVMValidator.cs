using ETicaret.UI.Areas.Admin.Models.ProductVMs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Models.Validators.ProductVMsValidators
{
    public class ProductUpdateVMValidator : AbstractValidator<ProductUpdateVM>
    {
        public ProductUpdateVMValidator(IStringLocalizer<ModelResource> localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["The product name cannot be ignored!"])
                .NotNull().WithMessage(localizer["The product name cannot be null!"]);
            RuleFor(x => x.UnitPrice).NotEmpty().WithMessage(localizer["The unit price cannot be ignored!"])
                .NotNull().WithMessage(localizer["The unit price cannot be null!"]);
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizer["The description cannot be ignored!"])
                .NotNull().WithMessage(localizer["The description cannot be null!"]);
            //RuleFor(x => x.NewImage)
            //   .NotEmpty().WithMessage(localizer["You must select an image."])
            //   .NotNull().WithMessage(localizer["The image cannot be null."]);
            //RuleFor(x => x.Quantity)
            //.NotEmpty().WithMessage(localizer["The quantity cannot be ignored!"])
            //.NotNull().WithMessage(localizer["The quantity cannot be null!"])
            //.GreaterThanOrEqualTo(0).WithMessage(localizer["The quantity cannot be negative!"]);
            //RuleFor(x => x.CategoryId)
            //.NotEmpty().WithMessage(localizer["The Category cannot be ignored!"])
            //.NotNull().WithMessage(localizer["The Category cannot be null!"]);
            //RuleFor(x => x.SizeId)
            //.NotEmpty().WithMessage(localizer["The Size cannot be ignored!"])
            //.NotNull().WithMessage(localizer["The Size  cannot be null!"]);

        }
    }
}
