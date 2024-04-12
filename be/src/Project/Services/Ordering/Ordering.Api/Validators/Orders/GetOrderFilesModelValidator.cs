using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.Orders;

namespace Ordering.Api.Validators.Orders
{
    public class GetOrderFilesModelValidator : BaseServiceModelValidator<GetOrderFilesServiceModel>
    {
        public GetOrderFilesModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
