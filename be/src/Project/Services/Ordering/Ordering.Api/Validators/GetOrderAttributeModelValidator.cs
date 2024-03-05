using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class GetOrderAttributeModelValidator : BaseServiceModelValidator<GetOrderAttributeServiceModel>
    {
        public GetOrderAttributeModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
