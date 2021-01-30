using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class GetOrderModelValidator : BaseServiceModelValidator<GetOrderServiceModel>
    {
        public GetOrderModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
