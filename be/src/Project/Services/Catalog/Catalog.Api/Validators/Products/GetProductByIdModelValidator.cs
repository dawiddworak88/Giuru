using Catalog.Api.ServicesModels.Products;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Products
{
    public class GetProductByIdModelValidator : BaseServiceModelValidator<GetProductByIdServiceModel>
    {
        public GetProductByIdModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
