using Analytics.Api.ServicesModels.SalesAnalytics;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Analytics.Api.Validators
{
    public class GetTopSalesAnalyticsModelValidator : BaseServiceModelValidator<GetTopSalesProductsAnalyticsServiceModel>
    {
        public GetTopSalesAnalyticsModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotEmpty().NotNull();
        }
    }
}
