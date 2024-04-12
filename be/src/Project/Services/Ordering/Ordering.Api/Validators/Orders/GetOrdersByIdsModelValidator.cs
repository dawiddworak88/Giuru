using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.Orders;

namespace Ordering.Api.Validators.Orders
{
    public class GetOrdersByIdsModelValidator : BaseServiceModelValidator<GetOrdersByIdsServiceModel>
    {
        public GetOrdersByIdsModelValidator()
        {
            RuleFor(x => x.OrganisationId).NotEmpty().NotNull();
            RuleFor(x => x.Ids).NotEmpty().NotNull();
        }
    }
}
