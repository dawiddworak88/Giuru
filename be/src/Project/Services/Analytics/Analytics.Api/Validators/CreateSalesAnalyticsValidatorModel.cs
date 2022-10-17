using Analytics.Api.ServicesModels.SalesAnalytics;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Analytics.Api.Validators
{
    public class CreateSalesAnalyticsValidatorModel : BaseServiceModelValidator<CreateSalesAnalyticsServiceModel>
    {
        public CreateSalesAnalyticsValidatorModel()
        {
            this.RuleFor(x => x.SalesAnalyticsItems).NotEmpty().NotNull();
        }
    }
}
