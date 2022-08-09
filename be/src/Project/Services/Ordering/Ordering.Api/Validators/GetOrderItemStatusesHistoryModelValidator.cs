using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class GetOrderItemStatusesHistoryModelValidator : BaseServiceModelValidator<GetOrderItemStatusesHistoryServiceModel>
    {
        public GetOrderItemStatusesHistoryModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
