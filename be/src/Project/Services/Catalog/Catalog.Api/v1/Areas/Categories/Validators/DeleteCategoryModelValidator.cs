using Catalog.Api.v1.Areas.Categories.Models;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.v1.Areas.Categories.Validators
{
    public class DeleteCategoryModelValidator : BaseServiceModelValidator<DeleteCategoryModel>
    {
        public DeleteCategoryModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull();
        }
    }
}
