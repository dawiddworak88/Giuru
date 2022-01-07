using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Organisations;

namespace Identity.Api.Validators
{
    public class GetSellerModelValidator : BaseServiceModelValidator<GetSellerModel>
    {
        public GetSellerModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
