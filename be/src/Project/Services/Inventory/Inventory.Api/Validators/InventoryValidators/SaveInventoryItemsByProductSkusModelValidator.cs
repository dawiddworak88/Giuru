using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.InventoryServices;

namespace Inventory.Api.Validators.InventoryValidators
{
    public class SaveInventoryItemsByProductSkusModelValidator : BaseServiceModelValidator<UpdateInventoryProductsServiceModel>
    {
        public SaveInventoryItemsByProductSkusModelValidator()
        {
            this.RuleFor(x => x.InventoryItems).NotNull().NotEmpty();
        }
    }
}
