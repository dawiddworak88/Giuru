using Basket.Api.ServicesModels;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Basket.Api.ServicesModelsValidators
{
    public class CheckoutBasketServiceModelValidator : BaseServiceModelValidator<CheckoutBasketServiceModel>
    {
        public CheckoutBasketServiceModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotNull().NotEmpty();
            this.RuleFor(x => x.ClientId).NotNull().NotEmpty();
            this.RuleFor(x => x.ClientName).NotNull().NotEmpty();
            this.RuleFor(x => x.BasketId).NotNull().NotEmpty();
            this.RuleFor(x => x.Username).NotNull().NotEmpty();
        }
    }
}
