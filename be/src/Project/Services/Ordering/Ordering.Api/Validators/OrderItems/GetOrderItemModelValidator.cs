using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.OrderItems;

namespace Ordering.Api.Validators.OrderItems
{
    public class GetOrderItemModelValidator : BaseServiceModelValidator<GetOrderItemServiceModel>
    {
        public GetOrderItemModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
