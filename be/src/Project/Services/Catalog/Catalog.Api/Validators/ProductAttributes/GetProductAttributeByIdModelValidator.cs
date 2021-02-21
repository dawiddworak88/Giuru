using Catalog.Api.ServicesModels.ProductAttributes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.ProductAttributes
{
    public class GetProductAttributeByIdModelValidator : BaseServiceModelValidator<GetProductAttributeByIdServiceModel>
    {
        public GetProductAttributeByIdModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
