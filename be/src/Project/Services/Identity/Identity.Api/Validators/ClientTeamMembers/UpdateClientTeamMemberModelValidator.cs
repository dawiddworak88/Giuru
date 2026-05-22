using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.ClientTeamMembers;

namespace Identity.Api.Validators.ClientTeamMembers
{
    public class UpdateClientTeamMemberModelValidator : BaseServiceModelValidator<UpdateClientTeamMemberServiceModel>
    {
        public UpdateClientTeamMemberModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
            this.RuleFor(x => x.Email).NotEmpty().NotNull();
            this.RuleFor(x => x.FirstName).NotEmpty().NotNull();
            this.RuleFor(x => x.LastName).NotEmpty().NotNull();
            this.RuleFor(x => x.OrganisationId).NotEmpty().NotNull();
        }
    }
}
