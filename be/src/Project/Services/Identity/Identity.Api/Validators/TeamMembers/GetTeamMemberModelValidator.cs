using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.TeamMembers;

namespace Identity.Api.Validators.TeamMembers
{
    public class GetTeamMemberModelValidator : BaseServiceModelValidator<GetTeamMemberServiceModel>
    {
        public GetTeamMemberModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
