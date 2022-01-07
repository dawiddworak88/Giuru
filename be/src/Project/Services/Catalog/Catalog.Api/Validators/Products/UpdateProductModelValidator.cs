using Catalog.Api.ServicesModels.Products;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Products
{
    public class UpdateProductModelValidator : BaseAuthorizedServiceModelValidator<CreateUpdateProductModel>
    {
        public UpdateProductModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull();
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
            this.RuleFor(x => x.CategoryId).NotNull();
        }
    }
}
