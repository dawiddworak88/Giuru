using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.OrderAttributes;

namespace Ordering.Api.Validators.OrderAttributes
{
    public class UpdateOrderAttributeModelValidator : BaseServiceModelValidator<UpdateOrderAttributeServiceModel>
    {
        public UpdateOrderAttributeModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
