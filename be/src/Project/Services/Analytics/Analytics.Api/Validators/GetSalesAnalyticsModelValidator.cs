using Analytics.Api.ServicesModels.SalesAnalytics;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Analytics.Api.Validators
{
    public class GetSalesAnalyticsModelValidator : BaseServiceModelValidator<GetSalesAnalyticsServiceModel>
    {
        public GetSalesAnalyticsModelValidator()
        {
            this.RuleFor(x => x.Username).NotEmpty().NotNull();
        }
    }
}
