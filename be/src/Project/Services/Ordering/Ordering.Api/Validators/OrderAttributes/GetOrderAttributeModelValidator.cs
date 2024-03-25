using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.OrderAttributes;

namespace Ordering.Api.Validators.OrderAttributes
{
    public class GetOrderAttributeModelValidator : BaseServiceModelValidator<GetOrderAttributeServiceModel>
    {
        public GetOrderAttributeModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
