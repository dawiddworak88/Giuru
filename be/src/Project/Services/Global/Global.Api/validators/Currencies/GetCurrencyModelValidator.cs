using FluentValidation;
using Foundation.Extensions.Validators;
using Global.Api.ServicesModels.Currencies;

namespace Global.Api.validators.Currencies
{
    public class GetCurrencyModelValidator : BaseServiceModelValidator<GetCurrencyServiceModel>
    {
        public GetCurrencyModelValidator() 
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
