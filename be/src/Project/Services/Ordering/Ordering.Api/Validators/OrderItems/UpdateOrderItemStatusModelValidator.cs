using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.OrderItems;

namespace Ordering.Api.Validators.OrderItems
{
    public class UpdateOrderItemStatusModelValidator : BaseServiceModelValidator<UpdateOrderItemStatusServiceModel>
    {
        public UpdateOrderItemStatusModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.OrderItemStatusId).NotEmpty().NotNull();
        }
    }
}
