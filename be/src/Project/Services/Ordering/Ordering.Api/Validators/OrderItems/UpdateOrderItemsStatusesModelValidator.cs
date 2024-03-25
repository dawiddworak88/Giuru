using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.OrderItems;

namespace Ordering.Api.Validators.OrderItems
{
    public class UpdateOrderItemsStatusesModelValidator : BaseServiceModelValidator<UpdateOrderItemsStatusesServiceModel>
    {
        public UpdateOrderItemsStatusesModelValidator()
        {
            RuleFor(x => x.OrderItems).NotEmpty().NotNull();
        }
    }
}
