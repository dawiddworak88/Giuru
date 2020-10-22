using Catalog.Api.v1.Areas.Products.Models;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.v1.Areas.Products.Validators
{
    public class GetProductSuggestionsModelValidator : BaseServiceModelValidator<GetProductSuggestionsModel>
    {
        public GetProductSuggestionsModelValidator()
        {
            RuleFor(x => x.SearchTerm).NotNull().NotEmpty();
            RuleFor(x => x.Size).LessThanOrEqualTo(100);
        }
    }
}
