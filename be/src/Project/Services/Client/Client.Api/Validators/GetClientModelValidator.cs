using Client.Api.ServicesModels;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators
{
    public class GetClientModelValidator : BaseServiceModelValidator<GetClientServiceModel>
    {
        public GetClientModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
