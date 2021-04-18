using Catalog.Api.ServicesModels.Categories;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Categories
{
    public class CreateCategoryModelValidator : BaseAuthorizedServiceModelValidator<CreateCategoryServiceModel>
    {
        public CreateCategoryModelValidator()
        {
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
