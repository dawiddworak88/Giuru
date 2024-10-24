﻿using FluentValidation;
using Foundation.Extensions.ExtensionMethods;
using Ordering.Api.ServicesModels;
using System;

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
