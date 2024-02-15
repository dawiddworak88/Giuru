using FluentValidation;
using Foundation.Extensions.Validators;
using Global.Api.ServicesModels.Countries;

namespace Global.Api.validators.Countries
{
    public class DeleteCountryModelValidator : BaseServiceModelValidator<DeleteCountryServiceModel>
    {
        public DeleteCountryModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
