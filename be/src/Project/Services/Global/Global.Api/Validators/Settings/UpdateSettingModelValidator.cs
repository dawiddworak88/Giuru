using FluentValidation;
using Foundation.Extensions.Validators;
using Global.Api.ServicesModels.Settings;

namespace Global.Api.validators.Settings
{
    public class UpdateSettingModelValidator : BaseServiceModelValidator<UpdateSettingServiceModel>
    {
        public UpdateSettingModelValidator()
        {
            RuleFor(x => x.SellerId).NotEmpty().NotNull();
        }
    }
}
