using Analytics.Api.ServicesModels.SalesAnalytics;
using FluentValidation;
using Foundation.Extensions.Validators;
using System;

namespace Analytics.Api.Validators
{
    public class GetCountriesSalesModelValidator : BaseServiceModelValidator<GetCountriesSalesServiceModel>
    {
        public GetCountriesSalesModelValidator()
        {
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
