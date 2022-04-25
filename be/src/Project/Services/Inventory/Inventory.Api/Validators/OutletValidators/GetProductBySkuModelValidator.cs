using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.OutletServiceModels;

namespace Inventory.Api.Validators.OutletValidators
{
    public class GetProductBySkuModelValidator : BaseServiceModelValidator<GetOutletByProductSkuServiceModel>
    {
        public GetProductBySkuModelValidator()
        {
            this.RuleFor(x => x.ProductSku).NotNull().NotEmpty();
        }
    }
}
