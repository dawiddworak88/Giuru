using Client.Api.ServicesModels.DiscountCodes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.DiscountCodes
{
    public class GetDiscountCodeModelValidator : BaseServiceModelValidator<GetDiscountCodeServiceModel>
    {
        public GetDiscountCodeModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.OrganisationId).NotNull().NotEmpty();
        }
    }
}
