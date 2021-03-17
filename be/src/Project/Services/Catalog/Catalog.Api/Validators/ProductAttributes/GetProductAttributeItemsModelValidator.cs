using Catalog.Api.ServicesModels.ProductAttributes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.ProductAttributes
{
    public class GetProductAttributeItemsModelValidator : BasePagedServiceModelValidator<GetProductAttributeItemsServiceModel>
    {
        public GetProductAttributeItemsModelValidator()
        {
            RuleFor(x => x.ProductAttributeId).NotNull().NotEmpty();
        }
    }
}
