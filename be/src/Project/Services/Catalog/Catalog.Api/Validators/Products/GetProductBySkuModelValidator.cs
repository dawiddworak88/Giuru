using Catalog.Api.ServicesModels.Products;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Products
{
    public class GetProductBySkuModelValidator : BaseServiceModelValidator<GetProductBySkuServiceModel>
    {
        public GetProductBySkuModelValidator()
        {
            RuleFor(x => x.Sku).NotNull().NotEmpty();
        }
    }
}
