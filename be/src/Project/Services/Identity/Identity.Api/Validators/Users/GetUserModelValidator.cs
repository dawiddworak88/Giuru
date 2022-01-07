using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Users;

namespace Identity.Api.Validators.Users
{
    public class GetUserModelValidator : BaseServiceModelValidator<GetUserServiceModel>
    {
        public GetUserModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
