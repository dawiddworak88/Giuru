using Client.Api.ServicesModels.Roles;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Roles
{
    public class UpdateClientRoleModelValidator : BaseServiceModelValidator<UpdateClientRoleServiceModel>
    {
        public UpdateClientRoleModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
            this.RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
