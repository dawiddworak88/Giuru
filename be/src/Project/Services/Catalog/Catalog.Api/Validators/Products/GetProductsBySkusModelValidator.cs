using Catalog.Api.ServicesModels.Products;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Products
{
    public class GetProductsBySkusModelValidator : BaseServiceModelValidator<GetProductsBySkusServiceModel>
    {
        public GetProductsBySkusModelValidator()
        {
            RuleFor(x => x.Skus).NotNull().NotEmpty();
        }
    }
}
