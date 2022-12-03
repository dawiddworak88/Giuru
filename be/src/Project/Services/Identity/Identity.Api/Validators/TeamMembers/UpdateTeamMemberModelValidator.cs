using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.TeamMembers;

namespace Identity.Api.Validators.TeamMembers
{
    public class UpdateTeamMemberModelValidator : BaseServiceModelValidator<UpdateTeamMemberServiceModel>
    {
        public UpdateTeamMemberModelValidator()
        {
            this.RuleFor(x => x.Email).NotEmpty().NotNull();
            this.RuleFor(x => x.FirstName).NotEmpty().NotNull();
            this.RuleFor(x => x.LastName).NotEmpty().NotNull();
        }
    }
}
