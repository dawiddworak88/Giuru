using DownloadCenter.Api.ServicesModels.Categories;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace DownloadCenter.Api.Validators.Categories
{
    public class GetCategoryModelValidator : BaseServiceModelValidator<GetCategoryServiceModel>
    {
        public GetCategoryModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
