using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class CreateOrderAttributeModelValidator : BaseServiceModelValidator<CreateOrderAttributeServiceModel>
    {
        public CreateOrderAttributeModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
