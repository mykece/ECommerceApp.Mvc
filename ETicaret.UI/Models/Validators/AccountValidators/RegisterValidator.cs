using ETicaret.UI.Models.AccountVMs;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Models.Validators.AccountValidators
{
    public class RegisterValidator : AbstractValidator<RegisterVM>
    {

        public RegisterValidator(IStringLocalizer<ModelResource> localizer)
        {
            RuleFor(x => x.Email).NotNull()
                                 .WithMessage(localizer["The e-mail field cannot be left blank."])
                                 .NotEmpty().WithMessage(localizer["The e-mail field cannot be left blank."]);
            RuleFor(x => x.Email).EmailAddress()
                                 .WithMessage(localizer["Please enter an email address in valid format."]);
            RuleFor(x => x.FirstName).NotNull()
                                 .WithMessage(localizer["First Name must be filled."]);
            RuleFor(x => x.LastName).NotNull()
                                    .WithMessage(localizer["Last Name must be filled."]);
            RuleFor(x => x.PhoneNumber).NotNull()
                                       .Must(BeNumeric)
                                       .MaximumLength(11)
                                       .WithMessage(localizer["Phone Number must be filled"]);
                                    
        }

        private bool BeNumeric(string phoneNumber)
        {
            return double.TryParse(phoneNumber, out _);
        }
    }

}
