using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Users;

namespace Identity.Api.Validators.Users
{
    public class CreateUserModelValidator : BaseServiceModelValidator<CreateUserServiceModel>
    {
        public CreateUserModelValidator()
        {
            this.RuleFor(x => x.Email).NotNull().NotEmpty();
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
            this.RuleFor(x => x.CommunicationsLanguage).NotNull().NotEmpty();
        }
    }
}