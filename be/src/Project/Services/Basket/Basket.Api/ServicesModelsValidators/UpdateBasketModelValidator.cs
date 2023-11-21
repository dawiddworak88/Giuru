using Basket.Api.ServicesModels;
using FluentValidation;
using Foundation.Extensions.Validators;
using System;
using System.Linq;

namespace Basket.Api.ServicesModelsValidators
{
    public class UpdateBasketModelValidator : BaseServiceModelValidator<UpdateBasketServiceModel>
    {
        public UpdateBasketModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.Items).Custom((items, context) => {

                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        if (!item.ProductId.HasValue || item.ProductId.Value == Guid.Empty)
                        {
                            context.AddFailure("Product id cannot be null or empty");
                        }

                        if (string.IsNullOrWhiteSpace(item.ProductSku))
                        {
                            context.AddFailure("Product SKU cannot be null or empty");
                        }

                        if (string.IsNullOrWhiteSpace(item.ProductName))
                        {
                            context.AddFailure("Product name cannot be null or empty");
                        }

                        var totalQuantity = item.Quantity + item.StockQuantity + item.OutletQuantity;
                        if (totalQuantity <= 0)
                        {
                            context.AddFailure("Total quantity must be greater than 0");
                        }
                    }
                }
            });
        }
    }
}
