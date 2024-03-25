using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.Orders;

namespace Ordering.Api.Validators.Orders
{
    public class UpdateOrderLinesStatusesModelValidator : BaseServiceModelValidator<UpdateOrderLinesStatusesServiceModel>
    {
        public UpdateOrderLinesStatusesModelValidator()
        {
            RuleFor(x => x.OrderItems).NotNull().NotEmpty();
        }
    }
}
