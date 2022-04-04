using FluentValidation;
using Foundation.Extensions.Validators;
using News.Api.ServicesModels.Categories;

namespace News.Api.Validators.Categories
{
    public class GetCategoryModelValidator : BaseServiceModelValidator<GetCategoryServiceModel>
    {
        public GetCategoryModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
