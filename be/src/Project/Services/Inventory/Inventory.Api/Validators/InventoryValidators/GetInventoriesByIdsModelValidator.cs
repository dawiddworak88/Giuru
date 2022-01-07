using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.InventoryServices;

namespace Inventory.Api.Validators.InventoryValidators
{
    public class GetInventoriesByIdsModelValidator : BaseServiceModelValidator<GetInventoriesByIdsServiceModel>
    {
        public GetInventoriesByIdsModelValidator()
        {
            RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
