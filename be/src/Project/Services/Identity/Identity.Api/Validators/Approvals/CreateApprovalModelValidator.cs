using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Approvals;

namespace Identity.Api.Validators.Approvals
{
    public class CreateApprovalModelValidator : BaseServiceModelValidator<CreateApprovalServiceModel>
    {
        public CreateApprovalModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
        }
    }
}
