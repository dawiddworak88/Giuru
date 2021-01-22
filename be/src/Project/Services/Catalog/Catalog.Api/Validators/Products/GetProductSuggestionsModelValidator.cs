using Catalog.Api.ServicesModels.Products;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Products
{
    public class GetProductSuggestionsModelValidator : BaseServiceModelValidator<GetProductSuggestionsServiceModel>
    {
        public GetProductSuggestionsModelValidator()
        {
            RuleFor(x => x.SearchTerm).NotNull().NotEmpty();
            RuleFor(x => x.Size).LessThanOrEqualTo(100);
        }
    }
}
