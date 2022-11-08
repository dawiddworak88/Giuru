using Analytics.Api.ServicesModels.SalesAnalytics;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Analytics.Api.Validators
{
    public class GetAnnualSalesModelValidator : BaseServiceModelValidator<GetAnnualSalesServiceModel>
    {
        public GetAnnualSalesModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotEmpty().NotNull();
        }
    }
}
