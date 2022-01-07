using Catalog.Api.ServicesModels.ProductAttributes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.ProductAttributes
{
    public class CreateProductAttributeItemModelValidator : BaseAuthorizedServiceModelValidator<CreateUpdateProductAttributeItemServiceModel>
    {
        public CreateProductAttributeItemModelValidator()
        {
            this.RuleFor(x => x.ProductAttributeId).NotNull().NotEmpty();
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
