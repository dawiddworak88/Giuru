using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class GetOrderAttributeOptionModelValidator : BaseServiceModelValidator<GetOrderAttributeOptionServiceModel>
    {
        public GetOrderAttributeOptionModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
