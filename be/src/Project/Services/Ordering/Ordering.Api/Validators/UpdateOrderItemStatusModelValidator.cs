using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class UpdateOrderItemStatusModelValidator : BaseServiceModelValidator<UpdateOrderItemStatusServiceModel>
    {
        public UpdateOrderItemStatusModelValidator()
        {
            this.RuleFor(x => x.OrderItemId).NotEmpty().NotNull();
            this.RuleFor(x => x.OrderStatusId).NotEmpty().NotNull();
        }
    }
}
