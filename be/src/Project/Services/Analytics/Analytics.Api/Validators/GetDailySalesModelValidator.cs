using Analytics.Api.ServicesModels.SalesAnalytics;
using Analytics.Api.Shared.Validators;
using FluentValidation;
using Foundation.Extensions.Validators;
using System;

namespace Analytics.Api.Validators
{
    public class GetDailySalesModelValidator : BaseChartServiceModelValidator<GetDailySalesServiceModel>
    {
        public GetDailySalesModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotEmpty().NotNull();
        }
    }
}
