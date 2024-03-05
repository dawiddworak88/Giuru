using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class DeleteOrderAttributeModelValidator : BaseServiceModelValidator<DeleteOrderAttributeServiceModel>
    {
        public DeleteOrderAttributeModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
