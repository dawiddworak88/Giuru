using Catalog.Api.ServicesModels.ProductAttributes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.ProductAttributes
{
    public class GetProductAttributeItemByIdModelValidator : BaseServiceModelValidator<GetProductAttributeItemByIdServiceModel>
    {
        public GetProductAttributeItemByIdModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
