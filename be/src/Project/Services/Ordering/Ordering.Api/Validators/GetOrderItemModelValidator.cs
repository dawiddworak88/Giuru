using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class GetOrderItemModelValidator : BaseServiceModelValidator<GetOrderItemServiceModel>
    {
        public GetOrderItemModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
