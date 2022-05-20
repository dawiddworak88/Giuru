using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.OutletServiceModels;

namespace Inventory.Api.Validators.OutletValidators
{
    public class SaveOutletItemsByProductSkusModelValidator : BaseServiceModelValidator<UpdateOutletProductsServiceModel>
    {
        public SaveOutletItemsByProductSkusModelValidator()
        {
            this.RuleFor(x => x.OutletItems).NotNull().NotEmpty();
        }
    }
}
