using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.InventoryServiceModels;

namespace Inventory.Api.Validators.InventoryValidators
{
    public class GetInventoriesByProductsIdsModelValidator : BaseServiceModelValidator<GetInventoriesByProductsIdsServiceModel>
    {
        public GetInventoriesByProductsIdsModelValidator()
        {
            RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
