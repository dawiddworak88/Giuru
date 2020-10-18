using Catalog.Api.v1.Areas.Products.Models;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.v1.Areas.Products.Validators
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
