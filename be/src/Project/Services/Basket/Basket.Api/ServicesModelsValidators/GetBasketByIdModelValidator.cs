using Basket.Api.ServicesModels;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Basket.Api.ServicesModelsValidators
{
    public class GetBasketByIdModelValidator : BaseServiceModelValidator<GetBasketByIdServiceModel>
    {
        public GetBasketByIdModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
