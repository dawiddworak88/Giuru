using Basket.Api.ServicesModels;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Basket.Api.ServicesModelsValidators
{
    public class GetBasketByOrganisationModelValidator : BaseServiceModelValidator<GetBasketByOrganisationServiceModel>
    {
        public GetBasketByOrganisationModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotNull().NotEmpty();
        }
    }
}
