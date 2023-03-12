using Analytics.Api.ServicesModels.SalesAnalytics;
using Analytics.Api.Shared.Validators;
using FluentValidation;

namespace Analytics.Api.Validators
{
    public class GetClientsSalesModelValidator : BaseChartServiceModelValidator<GetClientsSalesServiceModel>
    {
        public GetClientsSalesModelValidator() 
        {
        }
    }
}
