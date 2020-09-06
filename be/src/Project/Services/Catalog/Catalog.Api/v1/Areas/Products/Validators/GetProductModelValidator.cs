using Catalog.Api.v1.Areas.Products.Models;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.v1.Areas.Products.Validators
{
    public class GetProductModelValidator : BaseAuthorizedServiceModelValidator<GetProductModel>
    {
        public GetProductModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
