﻿using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.InventoryServiceModels;

namespace Inventory.Api.Validators.InventoryValidators
{
    public class GetProductBySkuModelValidator : BaseServiceModelValidator<GetInventoryByProductSkuServiceModel>
    {
        public GetProductBySkuModelValidator()
        {
            this.RuleFor(x => x.ProductSku).NotNull().NotEmpty();
        }
    }
}
