using Feature.Product.Models;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Feature.Product.Validators
{
    public class DeleteProductModelValidator : BaseServiceModelValidator<DeleteProductModel>
    {
        public DeleteProductModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
