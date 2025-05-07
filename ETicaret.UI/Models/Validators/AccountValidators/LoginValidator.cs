
using ETicaret.UI.Models.AccountVMs;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace ETicaret.UI.Models.Validators.AccountValidators
{
    public class LoginValidator : AbstractValidator<CustomerLoginVM>
    {
        public LoginValidator(IStringLocalizer<ModelResource> localizer)
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
