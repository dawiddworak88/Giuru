using FluentValidation;
using Foundation.Extensions.Validators;
using News.Api.ServicesModels.Categories;

namespace News.Api.Validators.Categories
{
    public class DeleteCategoryModelValidator : BaseServiceModelValidator<DeleteCategoryServiceModel>
    {
        public DeleteCategoryModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
