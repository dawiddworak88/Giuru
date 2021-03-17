using Catalog.Api.ServicesModels.ProductAttributes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.ProductAttributes
{
    public class CreateProductAttributeModelValidator : BaseAuthorizedServiceModelValidator<CreateUpdateProductAttributeServiceModel>
    {
        public CreateProductAttributeModelValidator()
        {
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
