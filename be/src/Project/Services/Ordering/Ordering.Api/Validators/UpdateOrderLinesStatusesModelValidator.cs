using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class UpdateOrderLinesStatusesModelValidator : BaseServiceModelValidator<UpdateOrderLinesStatusesServiceModel>
    {
        public UpdateOrderLinesStatusesModelValidator()
    {
            this.RuleFor(x => x.OrderItems).NotNull().NotEmpty();
    }
}
}
