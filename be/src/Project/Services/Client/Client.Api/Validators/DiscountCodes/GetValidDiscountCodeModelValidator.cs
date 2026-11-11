using Client.Api.ServicesModels.DiscountCodes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.DiscountCodes
{
    public class GetValidDiscountCodeModelValidator : BaseServiceModelValidator<GetValidDiscountCodeServiceModel>
    {
        public GetValidDiscountCodeModelValidator()
        {
            this.RuleFor(x => x.ClientId).NotNull().NotEmpty();
            this.RuleFor(x => x.Code).NotNull().NotEmpty();
            this.RuleFor(x => x.OrganisationId).NotNull().NotEmpty();
        }
    }
}
