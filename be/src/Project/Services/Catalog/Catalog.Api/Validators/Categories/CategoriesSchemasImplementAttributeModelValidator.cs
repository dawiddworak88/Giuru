using Catalog.Api.ServicesModels.Categories;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Categories
{
    public class CategoriesSchemasImplementAttributeModelValidator : BaseServiceModelValidator<CategoriesSchemasImplementAttributeServiceModel>
    {
        public CategoriesSchemasImplementAttributeModelValidator()
        {
            RuleFor(x => x.AttributeId).NotNull().NotEmpty();
        }
    }
}
