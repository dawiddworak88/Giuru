using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class DeleteOrderAttributeOptionModelValidator : BaseServiceModelValidator<DeleteOrderAttributeOptionServiceModel>
    {
        public DeleteOrderAttributeOptionModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
