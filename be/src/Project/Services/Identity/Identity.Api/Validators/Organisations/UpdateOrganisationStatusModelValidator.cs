using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Organisations;

namespace Identity.Api.Validators.Organisations
{
    public class UpdateOrganisationStatusModelValidator : BaseServiceModelValidator<UpdateOrganisationStatusServiceModel>
    {
        public UpdateOrganisationStatusModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
