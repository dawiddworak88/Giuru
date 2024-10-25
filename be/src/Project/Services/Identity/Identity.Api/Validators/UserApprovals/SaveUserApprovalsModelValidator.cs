using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.UserApprovals;

namespace Identity.Api.Validators.UserApprovals
{
    public class SaveUserApprovalsModelValidator : BaseServiceModelValidator<SaveUserApprovalsServiceModel>
    {
        public SaveUserApprovalsModelValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
        }
    }
}
