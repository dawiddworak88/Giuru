using Analytics.Api.ServicesModels.SalesAnalytics;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Analytics.Api.Validators
{
    public class GetAnnualProductSalesModelValidator : BaseServiceModelValidator<GetAnnualProductSalesServiceModel>
    {
        public GetAnnualProductSalesModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotEmpty().NotNull();
        }
    }
}
