using Catalog.Api.ServicesModels.Categories;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Categories
{
    public class GetCategorySchemaModelValidator : BaseServiceModelValidator<GetCategorySchemaServiceModel>
    {
        public GetCategorySchemaModelValidator()
        {
            this.RuleFor(x => x.CategoryId).NotNull().NotEmpty();
        }
    }
}
