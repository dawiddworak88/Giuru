using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.WarehouseServiceModels;

namespace Inventory.Api.Validators.WarehouseValidators
{
    public class GetWarehouseModelValidator : BaseServiceModelValidator<GetWarehouseServiceModel>
    {
        public GetWarehouseModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
