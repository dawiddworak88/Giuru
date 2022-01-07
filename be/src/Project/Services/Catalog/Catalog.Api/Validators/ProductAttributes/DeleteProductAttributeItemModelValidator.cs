using Catalog.Api.ServicesModels.ProductAttributes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.ProductAttributes
{
    public class DeleteProductAttributeItemModelValidator : BaseAuthorizedServiceModelValidator<DeleteProductAttributeItemServiceModel>
    {
        public DeleteProductAttributeItemModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
