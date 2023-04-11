using Analytics.Api.ServicesModels.SalesAnalytics;
using Analytics.Api.Shared.Validators;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Analytics.Api.Validators
{
    public class GetTopSalesProductsAnalyticsModelValidator : BaseChartServiceModelValidator<GetTopSalesProductsAnalyticsServiceModel>
    {
        public GetTopSalesProductsAnalyticsModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotEmpty().NotNull();
        }
    }
}
