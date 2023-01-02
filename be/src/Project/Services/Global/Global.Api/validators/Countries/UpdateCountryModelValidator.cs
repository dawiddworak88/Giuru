using FluentValidation;
using Foundation.Extensions.Validators;
using Global.Api.ServicesModels.Countries;

namespace Global.Api.validators.Countries
{
    public class UpdateCountryModelValidator : BaseServiceModelValidator<UpdateCountryServiceModel>
    {
        public UpdateCountryModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
            this.RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
