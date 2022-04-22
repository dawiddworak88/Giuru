using FluentValidation;
using Ordering.Api.ServicesModels;
using System;
using System.Linq;

namespace Ordering.Api.Validators
{
    public class CheckoutBasketServiceModelValidator : AbstractValidator<CheckoutBasketServiceModel>
    {
        public CheckoutBasketServiceModelValidator()
        {
            this.RuleFor(x => x.BasketId).NotNull().NotEmpty();
            this.RuleFor(x => x.ClientId).NotNull().NotEmpty();
            this.RuleFor(x => x.SellerId).NotNull().NotEmpty();
            this.RuleFor(x => x).Must(y =>
            {
                if (y.Items == null)
                {
                    return false;
                }

                if (!y.Items.Any())
                {
                    return false;
                }

                foreach (var item in y.Items)
                {
                    if (!item.ProductId.HasValue || item.ProductId.Value == Guid.Empty)
                    {
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(item.ProductSku))
                    {
                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(item.ProductName))
                    {
                        return false;
                    }

                    var totalQuantity = item.Quantity + item.StockQuantity + item.OutletQuantity;
                    if (totalQuantity <= 0)
                    {
                        return false;
                    }
                }

                return true;
            });
        }
    }
}
