using FluentValidation;
using Foundation.Extensions.Validators;
using Catalog.Api.v1.Areas.Taxonomies.Models;

namespace Catalog.Api.v1.Areas.Taxonomies.Validators
{
    public class GetTaxonomyModelValidator : BaseAuthorizedServiceModelValidator<GetTaxonomyModel>
    {
        public GetTaxonomyModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
