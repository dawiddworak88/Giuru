using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.ClientTeamMembers;

namespace Identity.Api.Validators.ClientTeamMembers
{
    public class CreateClientTeamMemberModelValidator : BaseServiceModelValidator<CreateClientTeamMemberServiceModel>
    {
        public CreateClientTeamMemberModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotEmpty().NotNull();
            this.RuleFor(x => x.Email).NotEmpty().NotNull();
            this.RuleFor(x => x.FirstName).NotEmpty().NotNull();
            this.RuleFor(x => x.LastName).NotEmpty().NotNull();
        }
    }
}
