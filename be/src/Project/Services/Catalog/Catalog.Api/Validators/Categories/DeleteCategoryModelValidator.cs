using Catalog.Api.ServicesModels.Categories;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Categories
{
    public class DeleteCategoryModelValidator : BaseAuthorizedServiceModelValidator<DeleteCategoryServiceModel>
    {
        public DeleteCategoryModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
