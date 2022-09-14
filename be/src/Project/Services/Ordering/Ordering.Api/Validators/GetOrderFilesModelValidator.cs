using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class GetOrderFilesModelValidator : BasePagedServiceModelValidator<GetOrderFilesServiceModel>
    {
        public GetOrderFilesModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
