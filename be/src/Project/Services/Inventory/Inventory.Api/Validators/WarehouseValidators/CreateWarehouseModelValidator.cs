using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.WarehouseServiceModels;

namespace Inventory.Api.Validators.WarehouseValidators
{
    public class CreateWarehouseModelValidator : BaseServiceModelValidator<CreateWarehouseServiceModel>
    {
        public CreateWarehouseModelValidator()
        {
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
            this.RuleFor(x => x.Location).NotNull().NotEmpty();
        }
    }
}
