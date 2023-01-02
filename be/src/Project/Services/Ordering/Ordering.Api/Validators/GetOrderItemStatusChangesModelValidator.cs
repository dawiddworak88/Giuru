using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class GetOrderItemStatusChangesModelValidator : BaseServiceModelValidator<GetOrderItemStatusChangesServiceModel>
    {
        public GetOrderItemStatusChangesModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
