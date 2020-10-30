using Catalog.Api.v1.Areas.Products.Models;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.v1.Areas.Products.Validators
{
    public class DeleteProductModelValidator : BaseAuthorizedServiceModelValidator<DeleteProductModel>
    {
        public DeleteProductModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
