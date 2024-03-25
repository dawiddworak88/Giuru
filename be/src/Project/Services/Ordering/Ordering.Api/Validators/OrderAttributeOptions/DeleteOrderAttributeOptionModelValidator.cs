using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.OrderAttributeOptions;

namespace Ordering.Api.Validators.OrderAttributeOptions
{
    public class DeleteOrderAttributeOptionModelValidator : BaseServiceModelValidator<DeleteOrderAttributeOptionServiceModel>
    {
        public DeleteOrderAttributeOptionModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
