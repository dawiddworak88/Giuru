using Client.Api.ServicesModels;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators
{
    public class UpdateClientModelValidator : BaseServiceModelValidator<UpdateClientServiceModel>
    {
        public UpdateClientModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
            this.RuleFor(x => x.ClientOrganisationId).NotNull().NotEmpty();
        }
    }
}
