using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.OutletServiceModels;

namespace Inventory.Api.Validators.OutletValidators
{
    public class CreateOutletModelValidator : BaseServiceModelValidator<CreateOutletServiceModel>
    {
        public CreateOutletModelValidator()
        {
            this.RuleFor(x => x.WarehouseId).NotNull().NotEmpty();
            this.RuleFor(x => x.ProductId).NotNull().NotEmpty();
            this.RuleFor(x => x.Quantity).NotNull().NotEmpty().GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.AvailableQuantity).NotNull().NotEmpty().GreaterThanOrEqualTo(0);
        }
    }
}
