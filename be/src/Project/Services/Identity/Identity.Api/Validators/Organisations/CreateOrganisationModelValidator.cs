using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Organisations;

namespace Identity.Api.Validators.Organisations
{
    public class CreateOrganisationModelValidator : BaseServiceModelValidator<CreateOrganisationServiceModel>
    {
        public CreateOrganisationModelValidator()
        {
            this.RuleFor(x => x.Email).NotNull().NotEmpty();
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
            this.RuleFor(x => x.CommunicationsLanguage).NotNull().NotEmpty();
        }
    }
}
