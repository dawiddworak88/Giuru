using Catalog.Api.v1.Areas.Categories.Models;
using FluentValidation;

namespace Catalog.Api.v1.Areas.Categories.Validators
{
    public class GetCategoriesModelValidator : AbstractValidator<GetCategoriesModel>
    {
        public GetCategoriesModelValidator()
        {
            RuleFor(x => x.Language).NotEmpty();
        }
    }
}
