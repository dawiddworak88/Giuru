using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class UpdateOrderAttributeOptionModelValidator : BaseServiceModelValidator<UpdateOrderAttributeOptionServiceModel>
    {
        public UpdateOrderAttributeOptionModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.OrderAttributeId).NotNull().NotEmpty();
        }
    }
}
