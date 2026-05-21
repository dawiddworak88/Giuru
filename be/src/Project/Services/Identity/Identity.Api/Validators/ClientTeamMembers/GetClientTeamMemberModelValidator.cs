using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.ClientTeamMembers;

namespace Identity.Api.Validators.ClientTeamMembers
{
    public class GetClientTeamMemberModelValidator : BaseServiceModelValidator<GetClientTeamMemberServiceModel>
    {
        public GetClientTeamMemberModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
