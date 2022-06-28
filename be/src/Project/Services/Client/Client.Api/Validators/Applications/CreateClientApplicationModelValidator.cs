using Client.Api.ServicesModels.Applications;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Applications
{
    public class CreateClientApplicationModelValidator : BaseServiceModelValidator<CreateClientApplicationServiceModel>
    {
        public CreateClientApplicationModelValidator()
        {
            this.RuleFor(x => x.Email).NotEmpty().NotNull();
            this.RuleFor(x => x.FirstName).NotEmpty().NotNull();
            this.RuleFor(x => x.LastName).NotEmpty().NotNull();
            this.RuleFor(x => x.PhoneNumber).NotEmpty().NotNull();
            this.RuleFor(x => x.ContactJobTitle).NotEmpty().NotNull();
            this.RuleFor(x => x.CompanyAddress).NotEmpty().NotNull();
            this.RuleFor(x => x.CompanyCity).NotEmpty().NotNull();
            this.RuleFor(x => x.CompanyCountry).NotEmpty().NotNull();
            this.RuleFor(x => x.CompanyName).NotEmpty().NotNull();
            this.RuleFor(x => x.CompanyRegion).NotEmpty().NotNull();
            this.RuleFor(x => x.CompanyPostalCode).NotEmpty().NotNull();
        }
    }
}
