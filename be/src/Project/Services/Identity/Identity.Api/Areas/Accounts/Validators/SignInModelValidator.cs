using Identity.Api.Areas.Accounts.Models;
using FluentValidation;

namespace Identity.Api.Areas.Accounts.Validators
{
    public class SignInModelValidator : AbstractValidator<SignInModel>
    {
        public SignInModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(8);
            RuleFor(x => x.ReturnUrl).NotNull().NotEmpty();
        }
    }
}
