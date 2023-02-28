using Analytics.Api.ServicesModels.SalesAnalytics;
using FluentValidation;
using Foundation.Extensions.Validators;
using System;

namespace Analytics.Api.Validators
{
    public class GetAnnualSalesModelValidator : BaseServiceModelValidator<GetAnnualSalesServiceModel>
    {
        public GetAnnualSalesModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotEmpty().NotNull();
            this.RuleFor(x => x).Must(y =>
            {
                if (y.FromDate > y.ToDate)
                {
                    return false;
                }

                var currentDate = DateTime.UtcNow;

                if ((y.FromDate > currentDate) && (y.ToDate > currentDate))
                {
                    return false;
                }

                return true;
            });
        }
    }
}
