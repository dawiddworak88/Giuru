using Client.Api.ServicesModels;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators
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