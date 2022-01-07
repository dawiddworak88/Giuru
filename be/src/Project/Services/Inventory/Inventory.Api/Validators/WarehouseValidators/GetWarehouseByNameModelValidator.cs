using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.WarehouseServices;

namespace Inventory.Api.Validators.WarehouseValidators
{
    public class GetWarehouseByNameModelValidator : BaseServiceModelValidator<GetWarehouseByNameServiceModel>
    {
        public GetWarehouseByNameModelValidator()
        {
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
