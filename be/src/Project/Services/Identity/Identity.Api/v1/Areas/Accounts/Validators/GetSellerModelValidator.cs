using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.v1.Areas.Accounts.Models;

namespace Identity.Api.v1.Areas.Accounts.Validators
{
    public class GetSellerModelValidator : BaseServiceModelValidator<GetSellerModel>
    {
        public GetSellerModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
