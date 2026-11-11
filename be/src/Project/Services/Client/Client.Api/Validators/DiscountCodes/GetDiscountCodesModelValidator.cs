using Client.Api.ServicesModels.DiscountCodes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.DiscountCodes
{
    public class GetDiscountCodesModelValidator : BaseServiceModelValidator<GetDiscountCodesServiceModel>
    {
        public GetDiscountCodesModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotNull().NotEmpty();
        }
    }
}
