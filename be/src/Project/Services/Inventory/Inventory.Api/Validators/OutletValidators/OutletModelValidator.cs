using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.OutletServices;

namespace Inventory.Api.Validators.OutletValidators
{
    public class OutletModelValidator : BaseServiceModelValidator<OutletServiceModel>
    {
        public OutletModelValidator()
        {
            this.RuleFor(x => x.ProductId).NotEmpty().NotNull();
            this.RuleFor(x => x.ProductName).NotEmpty().NotNull();
            this.RuleFor(x => x.ProductSku).NotEmpty().NotNull();
        }
    }
}
