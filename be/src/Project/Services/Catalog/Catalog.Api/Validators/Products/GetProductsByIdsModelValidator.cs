using Catalog.Api.ServicesModels.Products;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Products
{
    public class GetProductsByIdsModelValidator : BaseServiceModelValidator<GetProductsByIdsServiceModel>
    {
        public GetProductsByIdsModelValidator()
        {
            RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
