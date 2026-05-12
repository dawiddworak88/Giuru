using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.ClientTeamMembers;

namespace Identity.Api.Validators.ClientTeamMembers
{
    public class GetClientTeamMembersModelValidator : BasePagedServiceModelValidator<GetClientTeamMembersServiceModel>
    {
        public GetClientTeamMembersModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotEmpty().NotNull();
        }
    }
}
