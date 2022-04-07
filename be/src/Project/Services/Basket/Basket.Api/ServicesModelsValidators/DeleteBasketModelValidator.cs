using Basket.Api.ServicesModels;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Basket.Api.ServicesModelsValidators
{
    public class DeleteBasketModelValidator : BaseServiceModelValidator<DeleteBasketServiceModel>
    {
        public DeleteBasketModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
