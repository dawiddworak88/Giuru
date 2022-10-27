using Client.Api.ServicesModels.Clients;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Clients
{
    public class CreateClientModelValidator : BaseServiceModelValidator<CreateClientServiceModel>
    {
        public CreateClientModelValidator()
        {
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
            this.RuleFor(x => x.ClientOrganisationId).NotNull().NotEmpty();
        }
    }
}