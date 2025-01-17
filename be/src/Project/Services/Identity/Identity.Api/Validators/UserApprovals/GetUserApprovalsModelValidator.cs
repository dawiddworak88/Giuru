using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.UserApprovals;

namespace Identity.Api.Validators.UserApprovals
{
    public class GetUserApprovalsModelValidator : BaseServiceModelValidator<GetUserApprovalsServiceModel>
    {
        public GetUserApprovalsModelValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
        }
    }
}
