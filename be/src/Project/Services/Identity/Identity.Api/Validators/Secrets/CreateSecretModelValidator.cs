using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Secrets;

namespace Identity.Api.Validators.Secrets
{
    public class CreateSecretModelValidator : BaseServiceModelValidator<CreateSecretServiceModel>
    {
        public CreateSecretModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotEmpty().NotNull();
        }
    }
}
