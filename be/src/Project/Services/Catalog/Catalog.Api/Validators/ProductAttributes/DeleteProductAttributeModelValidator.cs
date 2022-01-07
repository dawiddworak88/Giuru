using Catalog.Api.ServicesModels.ProductAttributes;
using FluentValidation;
using Foundation.Extensions.Validators;
namespace Catalog.Api.Validators.ProductAttributes
{
    public class DeleteProductAttributeModelValidator : BaseAuthorizedServiceModelValidator<DeleteProductAttributeServiceModel>
    {
        public DeleteProductAttributeModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
