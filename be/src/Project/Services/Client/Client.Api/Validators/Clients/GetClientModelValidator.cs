using Client.Api.ServicesModels.Clients;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Clients
{
    public class GetClientModelValidator : BaseServiceModelValidator<GetClientServiceModel>
    {
        public GetClientModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
