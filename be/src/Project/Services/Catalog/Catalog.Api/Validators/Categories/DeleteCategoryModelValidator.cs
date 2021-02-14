using Catalog.Api.ServicesModels.Categories;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Categories
{
    public class DeleteCategoryModelValidator : BaseServiceModelValidator<DeleteCategoryServiceModel>
    {
        public DeleteCategoryModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull();
        }
    }
}
