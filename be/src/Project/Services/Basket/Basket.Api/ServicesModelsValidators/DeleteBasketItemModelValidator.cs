using Basket.Api.ServicesModels;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Basket.Api.ServicesModelsValidators
{
    public class DeleteBasketItemModelValidator : BaseServiceModelValidator<DeleteBasketItemServiceModel>
    {
        public DeleteBasketItemModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.OrganisationId).NotNull().NotEmpty();
        }
    }
}
