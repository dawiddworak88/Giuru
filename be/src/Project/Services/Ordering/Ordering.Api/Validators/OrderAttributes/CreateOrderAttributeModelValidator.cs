using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.OrderAttributes;

namespace Ordering.Api.Validators.OrderAttributes
{
    public class CreateOrderAttributeModelValidator : BaseServiceModelValidator<CreateOrderAttributeServiceModel>
    {
        public CreateOrderAttributeModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
