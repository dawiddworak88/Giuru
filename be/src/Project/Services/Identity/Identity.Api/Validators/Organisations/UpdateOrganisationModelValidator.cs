using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Organisations;

namespace Identity.Api.Validators.Organisations
{
    public class UpdateOrganisationModelValidator : BaseServiceModelValidator<UpdateOrganisationServiceModel>
    {
        public UpdateOrganisationModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
