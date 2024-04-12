using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.OrderAttributeOptions;

namespace Ordering.Api.Validators.OrderAttributeOptions
{
    public class CreateOrderAttributeOptionModelValidator : BaseServiceModelValidator<CreateOrderAttributeOptionServiceModel>
    {
        public CreateOrderAttributeOptionModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.OrderAttributeId).NotNull().NotEmpty();
        }
    }
}
