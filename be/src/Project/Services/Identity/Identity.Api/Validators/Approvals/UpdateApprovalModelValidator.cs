using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Approvals;

namespace Identity.Api.Validators.Approvals
{
    public class UpdateApprovalModelValidator : BaseServiceModelValidator<UpdateApprovalServiceModel>
    {
        public UpdateApprovalModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
        }
    }
}
