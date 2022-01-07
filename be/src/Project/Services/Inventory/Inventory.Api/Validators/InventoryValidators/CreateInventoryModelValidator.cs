using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.InventoryServices;

namespace Inventory.Api.Validators.InventoryValidators
{
    public class CreateInventoryModelValidator : BaseServiceModelValidator<CreateInventoryServiceModel>
    {
        public CreateInventoryModelValidator()
        {
            this.RuleFor(x => x.WarehouseId).NotNull().NotEmpty();
            this.RuleFor(x => x.ProductId).NotNull().NotEmpty();
            this.RuleFor(x => x.Quantity).NotNull().NotEmpty();
        }
    }
}
