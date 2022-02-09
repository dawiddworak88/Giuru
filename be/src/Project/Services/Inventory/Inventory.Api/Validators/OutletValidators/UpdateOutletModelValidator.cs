using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.OutletServices;

namespace Inventory.Api.Validators.OutletValidators
{
    public class UpdateOutletModelValidator : BaseServiceModelValidator<UpdateOutletServiceModel>
    {
        public UpdateOutletModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.ProductId).NotNull().NotEmpty();
            this.RuleFor(x => x.ProductName).NotNull().NotEmpty();
            this.RuleFor(x => x.ProductSku).NotNull().NotEmpty();
        }
    }
}
