using FluentValidation;
using Foundation.Extensions.Validators;
using Foundation.Taxonomy.Models;

namespace Foundation.Taxonomy.Validators
{
    public class GetTaxonomyModelValidator : BaseServiceModelValidator<GetTaxonomyModel>
    {
        public GetTaxonomyModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
