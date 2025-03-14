﻿using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.WarehouseServiceModels;

namespace Inventory.Api.Validators.WarehouseValidators
{
    public class GetWarehousesByIdsModelValidator : BaseServiceModelValidator<GetWarehousesByIdsServiceModel>
    {
        public GetWarehousesByIdsModelValidator()
        {
            RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
