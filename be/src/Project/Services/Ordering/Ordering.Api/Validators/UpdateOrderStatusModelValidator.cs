using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class UpdateOrderStatusModelValidator : BaseServiceModelValidator<UpdateOrderStatusServiceModel>
    {
        public UpdateOrderStatusModelValidator()
        {
            this.RuleFor(x => x.OrderId).NotNull().NotEmpty();
            this.RuleFor(x => x.OrderStatusId).NotNull().NotEmpty();
        }
    }
}
