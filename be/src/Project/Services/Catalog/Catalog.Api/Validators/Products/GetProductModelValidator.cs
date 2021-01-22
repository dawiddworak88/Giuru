using Catalog.Api.ServicesModels.Products;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Products
{
    public class GetProductModelValidator : BaseServiceModelValidator<GetProductServiceModel>
    {
        public GetProductModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
