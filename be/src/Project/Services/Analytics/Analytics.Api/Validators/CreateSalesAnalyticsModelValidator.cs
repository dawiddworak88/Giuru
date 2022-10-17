using Analytics.Api.ServicesModels.SalesAnalytics;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Analytics.Api.Validators
{
    public class CreateSalesAnalyticsModelValidator : BaseServiceModelValidator<CreateSalesAnalyticsServiceModel>
    {
        public CreateSalesAnalyticsModelValidator()
        {
            this.RuleFor(x => x.SalesAnalyticsItems).NotEmpty().NotNull();
        }
    }
}
