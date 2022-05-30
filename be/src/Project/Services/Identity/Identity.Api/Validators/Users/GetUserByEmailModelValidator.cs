using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Users;

namespace Identity.Api.Validators.Users
{
    public class GetUserByEmailModelValidator : BaseServiceModelValidator<GetUserByEmailServiceModel>
    {
        public GetUserByEmailModelValidator()
        {
            this.RuleFor(x => x.Email).NotNull().NotEmpty();
        }
    }
}
