using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Users;

namespace Identity.Api.Validators.Users
{
    public class ResetUserPasswordModelValidator : BaseServiceModelValidator<ResetUserPasswordServiceModel>
    {
        public ResetUserPasswordModelValidator()
        {
            this.RuleFor(x => x.Email).NotNull().NotEmpty();
        }
    }
}
