using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class CreateBatchOrderAttributeValuesModelValidator : BaseServiceModelValidator<CreateBatchOrderAttributeValuesServiceModel>
    {
        public CreateBatchOrderAttributeValuesModelValidator()
        {
            RuleFor(x => x.OrderId).NotNull().NotEmpty();
            RuleFor(x => x.Values).NotNull().NotEmpty();
        }
    }
}
