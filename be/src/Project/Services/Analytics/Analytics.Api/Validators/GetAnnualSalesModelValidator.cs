using Analytics.Api.ServicesModels.SalesAnalytics;
using Analytics.Api.Shared.Validators;
using FluentValidation;
using System;

namespace Analytics.Api.Validators
{
    public class GetAnnualSalesModelValidator : BaseChartServiceModelValidator<GetAnnualSalesServiceModel>
    {
        public GetAnnualSalesModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotEmpty().NotNull();
        }
    }
}
