using FluentValidation;
using Foundation.Extensions.ExtensionMethods;
using Ordering.Api.ServicesModels.Orders;
using System;

namespace Ordering.Api.Validators.Orders
{
    public class CheckoutBasketServiceModelValidator : AbstractValidator<CheckoutBasketServiceModel>
    {
        public CheckoutBasketServiceModelValidator()
        {
            RuleFor(x => x.BasketId).NotNull().NotEmpty();
            RuleFor(x => x.ClientId).NotNull().NotEmpty();
            RuleFor(x => x.SellerId).NotNull().NotEmpty();
            RuleFor(x => x).Must(y =>
            {
                foreach (var item in y.Items.OrEmptyIfNull())
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

                if (y.HasCustomOrder)
                {
                    if (string.IsNullOrWhiteSpace(y.MoreInfo))
                    {
                        return false;
                    }
                }

                return true;
            });
        }
    }
}
