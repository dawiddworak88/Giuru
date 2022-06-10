using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Applications;

namespace Identity.Api.Areas.Accounts.Validators
{
    public class CreateClientApplicationModelValidator : BaseServiceModelValidator<CreateClientApplicationServiceModel>
    {
        public CreateClientApplicationModelValidator()
        {
            this.RuleFor(x => x.FirstName).NotNull().NotEmpty();
            this.RuleFor(x => x.LastName).NotNull().NotEmpty();
            this.RuleFor(x => x.ContactJobTitle).NotNull().NotEmpty();
            this.RuleFor(x => x.Email).NotNull().NotEmpty();
            this.RuleFor(x => x.PhoneNumber).NotNull().NotEmpty();
            this.RuleFor(x => x.CompanyName).NotNull().NotEmpty();
            this.RuleFor(x => x.CompanyAddress).NotNull().NotEmpty();
            this.RuleFor(x => x.CompanyCity).NotNull().NotEmpty();
            this.RuleFor(x => x.CompanyCountry).NotNull().NotEmpty();
            this.RuleFor(x => x.CompanyPostalCode).NotNull().NotEmpty();
            this.RuleFor(x => x.CompanyRegion).NotNull().NotEmpty();
        }
    }
}
