using Catalog.Api.ServicesModels.Categories;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Categories
{
    public class UpdateCategorySchemaModelValidator : BaseAuthorizedServiceModelValidator<UpdateCategorySchemaServiceModel>
    {
        public UpdateCategorySchemaModelValidator()
        {
            this.RuleFor(x => x.CategoryId).NotNull().NotEmpty();
            this.RuleFor(x => x.Schema).NotNull().NotEmpty();
        }
    }
}
