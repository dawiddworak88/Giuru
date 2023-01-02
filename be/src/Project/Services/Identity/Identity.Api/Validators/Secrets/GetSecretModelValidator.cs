using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Secrets;

namespace Identity.Api.Validators.Secrets
{
    public class GetSecretModelValidator : BaseServiceModelValidator<GetSecretServiceModel>
    {
        public GetSecretModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotEmpty().NotNull();
        }
    }
}
