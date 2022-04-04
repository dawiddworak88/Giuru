using FluentValidation;
using Foundation.Extensions.Validators;
using News.Api.ServicesModels.Categories;

namespace News.Api.Validators.Categories
{
    public class UpdateCategoryModelValidator : BaseServiceModelValidator<UpdateCategoryServiceModel>
    {
        public UpdateCategoryModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
