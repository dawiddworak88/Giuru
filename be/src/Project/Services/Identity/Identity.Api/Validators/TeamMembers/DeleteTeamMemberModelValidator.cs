using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.TeamMembers;

namespace Identity.Api.Validators.TeamMembers
{
    public class DeleteTeamMemberModelValidator : BaseServiceModelValidator<DeleteTeamMemberServiceModel>
    {
        public DeleteTeamMemberModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
