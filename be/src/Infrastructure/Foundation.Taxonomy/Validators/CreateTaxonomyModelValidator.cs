using FluentValidation;
using Foundation.Extensions.Validators;
using Foundation.Taxonomy.Models;

namespace Foundation.Taxonomy.Validators
{
    public class CreateTaxonomyModelValidator : BaseServiceModelValidator<CreateTaxonomyModel>
    {
        public CreateTaxonomyModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
