using FluentValidation;
using Identity.Api.Areas.Accounts.ApiRequestModels;
using Identity.Api.ServicesModels.Users;

namespace Identity.Api.Areas.Accounts.Validators
{
    public class SetPasswordModelValidator : AbstractValidator<SetUserPasswordServiceModel>
    {
        public SetPasswordModelValidator()
        {
            this.RuleFor(x => x.ExpirationId).NotNull().NotEmpty();
            this.RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(8);
        }
    }
}
