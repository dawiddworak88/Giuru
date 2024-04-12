using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.OrderAttributes;

namespace Ordering.Api.Validators.OrderAttributes
{
    public class DeleteOrderAttributeModelValidator : BaseServiceModelValidator<DeleteOrderAttributeServiceModel>
    {
        public DeleteOrderAttributeModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
