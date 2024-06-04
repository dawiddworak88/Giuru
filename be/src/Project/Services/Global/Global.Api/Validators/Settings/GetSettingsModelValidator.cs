using FluentValidation;
using Foundation.Extensions.Validators;
using Global.Api.ServicesModels.Settings;

namespace Global.Api.validators.Settings
{
    public class GetSettingsModelValidator : BaseServiceModelValidator<GetSettingsServiceModel>
    {
        public GetSettingsModelValidator()
        {
            RuleFor(x => x.SellerId).NotEmpty().NotNull();
        }
    }
}
