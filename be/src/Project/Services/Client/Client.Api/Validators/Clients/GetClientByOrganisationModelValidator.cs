using Client.Api.ServicesModels.Clients;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Clients
{
    public class GetClientByOrganisationModelValidator : BaseServiceModelValidator<GetClientByOrganisationServiceModel>
    {
        public GetClientByOrganisationModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
