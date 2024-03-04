using FluentValidation;
using Foundation.Extensions.Validators;
using Global.Api.ServicesModels.Currencies;

namespace Global.Api.validators.Currencies
{
    public class DeleteCurrencyModelValidator : BaseServiceModelValidator<DeleteCurrencyServiceModel>
    {
        public DeleteCurrencyModelValidator() 
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
