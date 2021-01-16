using Basket.Api.v1.Areas.Baskets.Models;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Basket.Api.v1.Areas.Baskets.Validators
{
    public class CheckoutBasketServiceModelValidator : BaseServiceModelValidator<CheckoutBasketServiceModel>
    {
        public CheckoutBasketServiceModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotNull().NotEmpty();
            this.RuleFor(x => x.ClientId).NotNull().NotEmpty();
            this.RuleFor(x => x.BasketId).NotNull().NotEmpty();
            this.RuleFor(x => x.Username).NotNull().NotEmpty();
        }
    }
}
