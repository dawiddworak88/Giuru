using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Organisations;

namespace Identity.Api.Validators.Organisations
{
    public class GetOrganisationModelValidator : BaseServiceModelValidator<GetOrganisationModel>
    {
        public GetOrganisationModelValidator()
        {
            this.RuleFor(x => x.Email).NotNull().NotEmpty();
        }
    }
}
