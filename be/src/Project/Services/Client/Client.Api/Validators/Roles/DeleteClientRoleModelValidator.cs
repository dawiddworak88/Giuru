using Client.Api.ServicesModels.Roles;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Roles
{
    public class DeleteClientRoleModelValidator : BaseServiceModelValidator<DeleteClientRoleServiceModel>
    {
        public DeleteClientRoleModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
