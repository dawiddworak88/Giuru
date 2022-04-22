using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.WarehouseServiceModels;

namespace Inventory.Api.Validators.WarehouseValidators
{
    public class UpdateWarehouseModelValidator : BaseServiceModelValidator<UpdateWarehouseServiceModel>
    {
        public UpdateWarehouseModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
            this.RuleFor(x => x.Location).NotNull().NotEmpty();
        }
    }
}
