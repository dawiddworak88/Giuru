using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.InventoryServices;

namespace Inventory.Api.Validators.InventoryValidators
{
    public class UpdateInventoryModelValidator : BaseServiceModelValidator<UpdateInventoryServiceModel>
    {
        public UpdateInventoryModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.WarehouseId).NotNull().NotEmpty();
            this.RuleFor(x => x.ProductId).NotNull().NotEmpty();
            this.RuleFor(x => x.Quantity).NotNull().NotEmpty();
        }
    }
}
