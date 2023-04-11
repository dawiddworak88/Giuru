using Analytics.Api.Shared.ServicesModels;
using FluentValidation;
using Foundation.Extensions.Validators;
using System;

namespace Analytics.Api.Shared.Validators
{
    public class BaseChartServiceModelValidator<T> : BaseServiceModelValidator<T> where T : ChartBaseServiceModel
    {
        public BaseChartServiceModelValidator()
        {
            this.RuleFor(x => x).Must(y =>
            {
                if (y.FromDate > y.ToDate || y.FromDate > DateTime.UtcNow)
                {
                    return false;
                }

                return true;
            });
        }
    }
}
