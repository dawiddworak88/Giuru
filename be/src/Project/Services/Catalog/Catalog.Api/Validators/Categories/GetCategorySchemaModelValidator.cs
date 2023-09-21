using Catalog.Api.ServicesModels.Categories;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Categories
{
    public class GetCategorySchemaModelValidator : BaseServiceModelValidator<GetCategorySchemaServiceModel>
    {
        public GetCategorySchemaModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
