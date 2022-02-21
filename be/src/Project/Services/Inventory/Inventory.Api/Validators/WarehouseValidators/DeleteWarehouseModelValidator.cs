using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.WarehouseServiceModels;

namespace Inventory.Api.Validators.WarehouseValidators
{
    public class DeleteWarehouseModelValidator : BaseServiceModelValidator<DeleteWarehouseServiceModel>
    {
        public DeleteWarehouseModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull();
        }
    }
}
