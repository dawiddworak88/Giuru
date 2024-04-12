using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.Orders;

namespace Ordering.Api.Validators.Orders
{
    public class GetOrderModelValidator : BaseServiceModelValidator<GetOrderServiceModel>
    {
        public GetOrderModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
