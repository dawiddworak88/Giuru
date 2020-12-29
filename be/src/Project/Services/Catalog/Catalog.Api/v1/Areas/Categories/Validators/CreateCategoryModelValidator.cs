using Catalog.Api.v1.Areas.Categories.Models;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.v1.Areas.Categories.Validators
{
    public class CreateCategoryModelValidator : BaseServiceModelValidator<CreateCategoryModel>
    {
        public CreateCategoryModelValidator()
        {
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
