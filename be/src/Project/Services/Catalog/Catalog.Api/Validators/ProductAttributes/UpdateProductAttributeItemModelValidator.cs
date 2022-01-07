using Catalog.Api.ServicesModels.ProductAttributes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.ProductAttributes
{
    public class UpdateProductAttributeItemModelValidator : BaseAuthorizedServiceModelValidator<CreateUpdateProductAttributeItemServiceModel>
    {
        public UpdateProductAttributeItemModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
