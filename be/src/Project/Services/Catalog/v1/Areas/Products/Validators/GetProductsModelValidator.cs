using Catalog.Api.v1.Areas.Products.Models;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.v1.Areas.Products.Validators
{
    public class GetProductsModelValidator : BaseServiceModelValidator<GetProductsModel>
    {
        public GetProductsModelValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(1);
            RuleFor(x => x.ItemsPerPage).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
        }
    }
}
