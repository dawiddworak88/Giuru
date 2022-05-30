using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.InventoryServiceModels;

namespace Inventory.Api.Validators.InventoryValidators
{
    public class GetProductByIdModelValidator : BaseServiceModelValidator<GetInventoryByProductIdServiceModel>
    {
        public GetProductByIdModelValidator()
        {
            this.RuleFor(x => x.ProductId.Value).NotNull().NotEmpty();
        }
    }
}
