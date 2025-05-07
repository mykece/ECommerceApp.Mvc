using ETicaret.UI.Areas.Admin.Models.CampaignVMs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Models.Validators.CampaignVMsValidators
{
    public class CampaignCreateVMValidator : AbstractValidator<CampaignCreateVM>
    {
        public CampaignCreateVMValidator(IStringLocalizer<ModelResource> localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer["The campaign name cannot be ignored!"])
                .NotNull().WithMessage(localizer["The campaign name cannot be null!"]);

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage(localizer["The start date cannot be ignored!"])
                .NotNull().WithMessage(localizer["The start date cannot be null!"]);

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage(localizer["The end date cannot be ignored!"])
                .NotNull().WithMessage(localizer["The end date cannot be null!"])
                .GreaterThan(x => x.StartDate).WithMessage(localizer["The end date must be later than the start date!"]);

            RuleFor(x => x.DiscountPercentage)
               .InclusiveBetween(0, 100).WithMessage(localizer["The discount percentage must be between 0 and 100!"]);
            RuleFor(x => x.NewImage)
               .NotEmpty().WithMessage(localizer["You must select an image."])
               .NotNull().WithMessage(localizer["The image cannot be null."]);

            // Kampanya'nın Create metodunda modelin validasyon kontrolü eksikti. Onu ekledim. Create işlemi sırasında ürün seçilmesine rağmen seçilen ürünleri görmüyor. O yüzden bu validasyon kuralını yoruma aldım. /Burak

            //RuleFor(x => x.ProductIds)
            //    .NotEmpty().WithMessage(localizer["At least one product must be selected!"])
            //    .Must(x => x != null && x.Any()).WithMessage(localizer["At least one product must be selected!"]);

        }
    }
}
