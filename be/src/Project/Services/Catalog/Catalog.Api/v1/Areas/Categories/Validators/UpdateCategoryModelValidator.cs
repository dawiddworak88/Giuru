using Catalog.Api.v1.Areas.Categories.Models;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.v1.Areas.Categories.Validators
{
    public class UpdateCategoryModelValidator : BaseServiceModelValidator<UpdateCategoryModel>
    {
        public UpdateCategoryModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
