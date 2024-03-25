using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.Orders;

namespace Ordering.Api.Validators.Orders
{
    public class GetOrdersModelValidator : BasePagedServiceModelValidator<GetOrdersServiceModel>
    {
        public GetOrdersModelValidator()
        {
            RuleFor(x => x.OrganisationId).NotNull().NotEmpty();
        }
    }
}
