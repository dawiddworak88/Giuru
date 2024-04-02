using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.OrderAttributeValues;

namespace Ordering.Api.Validators.OrderAttributeValues
{
    public class CreateBatchOrderAttributeValuesModelValidator : BaseServiceModelValidator<CreateBatchOrderAttributeValuesServiceModel>
    {
        public CreateBatchOrderAttributeValuesModelValidator()
        {
            RuleFor(x => x.Values).NotNull().NotEmpty();
        }
    }
}
