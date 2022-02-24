using FluentValidation;
using Foundation.Extensions.Validators;
using News.Api.ServicesModels.Categories;

namespace News.Api.Validators.Categories
{
    public class CreateCategoryModelValidator : BaseServiceModelValidator<CreateCategoryServiceModel>
    {
        public CreateCategoryModelValidator()
        {
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
