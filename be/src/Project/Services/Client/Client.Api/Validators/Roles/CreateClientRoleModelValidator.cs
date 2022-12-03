using Client.Api.ServicesModels.Roles;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Roles
{
    public class CreateClientRoleModelValidator : BaseServiceModelValidator<CreateClientRoleServiceModel>
    {
        public CreateClientRoleModelValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
