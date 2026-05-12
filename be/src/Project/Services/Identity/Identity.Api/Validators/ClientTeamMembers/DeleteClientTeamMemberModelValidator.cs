using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.ClientTeamMembers;

namespace Identity.Api.Validators.ClientTeamMembers
{
    public class DeleteClientTeamMemberModelValidator : BaseServiceModelValidator<DeleteClientTeamMemberServiceModel>
    {
        public DeleteClientTeamMemberModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
