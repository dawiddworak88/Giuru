using Catalog.Api.ServicesModels.Products;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Products
{
    public class CreateProductModelValidator : BaseAuthorizedServiceModelValidator<CreateUpdateProductModel>
    {
        public CreateProductModelValidator()
        {
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
            this.RuleFor(x => x.CategoryId).NotNull();
        }
    }
}
