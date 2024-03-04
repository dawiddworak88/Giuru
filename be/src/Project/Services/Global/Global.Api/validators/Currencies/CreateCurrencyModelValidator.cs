using FluentValidation;
using Foundation.Extensions.Validators;
using Global.Api.ServicesModels.Currencies;
using System.Data;

namespace Global.Api.validators.Currencies
{
    public class CreateCurrencyModelValidator : BaseServiceModelValidator<CreateCurrencyServiceModel>
    {
        public CreateCurrencyModelValidator()
        {
            RuleFor(x => x.CurrencyCode).NotEmpty().NotNull();
            RuleFor(x => x.Symbol).NotEmpty().NotNull();
        }
    }
}
