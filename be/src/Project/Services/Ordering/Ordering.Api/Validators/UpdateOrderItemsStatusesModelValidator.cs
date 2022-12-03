using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class UpdateOrderItemsStatusesModelValidator : BaseServiceModelValidator<UpdateOrderItemsStatusesServiceModel>
    {
        public UpdateOrderItemsStatusesModelValidator()
        {
            this.RuleFor(x => x.OrderItems).NotEmpty().NotNull();
        }
    }
}
