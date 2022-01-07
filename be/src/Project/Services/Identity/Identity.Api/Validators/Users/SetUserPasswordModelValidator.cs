using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Users;

namespace Identity.Api.Validators.Users
{
    public class SetUserPasswordModelValidator : BaseServiceModelValidator<SetUserPasswordServiceModel>
    {
        public SetUserPasswordModelValidator()
        {
            this.RuleFor(x => x.Password).NotNull().NotEmpty();
            this.RuleFor(x => x.ExpirationId).NotNull().NotEmpty();
        }
    }
}
