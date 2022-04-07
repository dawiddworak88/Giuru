using Client.Api.ServicesModels;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators
{
    public class GetClientByOrganisationModelValidator : BaseServiceModelValidator<GetClientByOrganisationServiceModel>
    {
        public GetClientByOrganisationModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
