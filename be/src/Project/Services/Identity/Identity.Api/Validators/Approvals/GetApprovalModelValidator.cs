using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Approvals;

namespace Identity.Api.Validators.Approvals
{
    public class GetApprovalModelValidator : BaseServiceModelValidator<GetApprovalServiceModel>
    {
        public GetApprovalModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
