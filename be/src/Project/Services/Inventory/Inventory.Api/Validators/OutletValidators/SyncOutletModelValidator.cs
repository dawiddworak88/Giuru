using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.OutletServices;

namespace Inventory.Api.Validators.OutletValidators
{
    public class SyncOutletModelValidator : BaseServiceModelValidator<SyncOutletServiceModel>
    {
        public SyncOutletModelValidator()
        {
            this.RuleFor(x => x.OutletItems).NotNull().NotEmpty();
        }
    }
}
