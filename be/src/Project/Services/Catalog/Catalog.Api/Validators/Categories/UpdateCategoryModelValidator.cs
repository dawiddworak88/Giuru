using Catalog.Api.ServicesModels.Categories;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Categories
{
    public class UpdateCategoryModelValidator : BaseAuthorizedServiceModelValidator<UpdateCategoryServiceModel>
    {
        public UpdateCategoryModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
