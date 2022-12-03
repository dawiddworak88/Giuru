using Client.Api.ServicesModels.Roles;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Roles
{
    public class GetClientRoleModelValidator : BaseServiceModelValidator<GetClientRoleServiceModel>
    {
        public GetClientRoleModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
