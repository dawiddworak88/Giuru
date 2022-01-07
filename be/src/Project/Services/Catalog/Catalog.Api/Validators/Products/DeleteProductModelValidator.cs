using Catalog.Api.ServicesModels.Products;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Products
{
    public class DeleteProductModelValidator : BaseAuthorizedServiceModelValidator<DeleteProductServiceModel>
    {
        public DeleteProductModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
