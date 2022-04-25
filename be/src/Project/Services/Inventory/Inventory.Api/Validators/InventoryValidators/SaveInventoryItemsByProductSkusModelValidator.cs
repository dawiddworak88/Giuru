using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.InventoryServiceModels;

namespace Inventory.Api.Validators.InventoryValidators
{
    public class SaveInventoryItemsByProductSkusModelValidator : BaseServiceModelValidator<UpdateProductsInventoryServiceModel>
    {
        public SaveInventoryItemsByProductSkusModelValidator()
        {
            this.RuleFor(x => x.InventoryItems).NotNull().NotEmpty();
        }
    }
}
