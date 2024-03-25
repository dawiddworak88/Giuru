using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.Orders;

namespace Ordering.Api.Validators.Orders
{
    public class UpdateOrderStatusModelValidator : BaseServiceModelValidator<UpdateOrderStatusServiceModel>
    {
        public UpdateOrderStatusModelValidator()
        {
            RuleFor(x => x.OrderId).NotNull().NotEmpty();
            RuleFor(x => x.OrderStatusId).NotNull().NotEmpty();
        }
    }
}
