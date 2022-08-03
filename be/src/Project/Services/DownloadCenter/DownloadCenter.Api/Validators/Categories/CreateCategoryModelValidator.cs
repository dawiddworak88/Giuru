using DownloadCenter.Api.ServicesModels.Categories;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace DownloadCenter.Api.Validators.Categories
{
    public class CreateCategoryModelValidator : BaseServiceModelValidator<CreateCategoryServiceModel>
    {
        public CreateCategoryModelValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
