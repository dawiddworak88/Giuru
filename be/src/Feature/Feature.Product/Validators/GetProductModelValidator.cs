using Feature.Product.Models;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Feature.Product.Validators
{
    public class GetProductModelValidator : BaseServiceModelValidator<GetProductModel>
    {
        public GetProductModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
