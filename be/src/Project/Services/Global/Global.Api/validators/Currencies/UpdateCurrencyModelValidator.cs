using FluentValidation;
using Foundation.Extensions.Validators;
using Global.Api.ServicesModels.Currencies;

namespace Global.Api.validators.Currencies
{
    public class UpdateCurrencyModelValidator : BaseServiceModelValidator<UpdateCurrencyServiceModel>
    {
        public UpdateCurrencyModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.CurrencyCode).NotEmpty().NotNull();
            RuleFor(x => x.Symbol).NotEmpty().NotNull();
        }
    }
}
