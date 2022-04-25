using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Roles;

namespace Identity.Api.Validators.Roles
{
    public class CreateRolesModelValidator : BaseServiceModelValidator<CreateRolesServiceModel>
    {
        public CreateRolesModelValidator()
        {
            this.RuleFor(x => x.Email).NotEmpty().NotNull();
            this.RuleFor(x => x.Roles).NotEmpty().NotNull();
        }
    }
}
