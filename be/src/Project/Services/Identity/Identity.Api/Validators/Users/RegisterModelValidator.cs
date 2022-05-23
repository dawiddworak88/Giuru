using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Users;

namespace Identity.Api.Validators.Users
{
    public class RegisterModelValidator : BaseServiceModelValidator<RegisterServiceModel>
    {
        public RegisterModelValidator()
        {
            this.RuleFor(x => x.FirstName).NotNull().NotEmpty();
            this.RuleFor(x => x.LastName).NotNull().NotEmpty();
            this.RuleFor(x => x.ContactJobTitle).NotNull().NotEmpty();
            this.RuleFor(x => x.Email).NotNull().NotEmpty();
            this.RuleFor(x => x.PhoneNumber).NotNull().NotEmpty();
            this.RuleFor(x => x.CompanyName).NotNull().NotEmpty();
            this.RuleFor(x => x.CompanyAddress).NotNull().NotEmpty();
            this.RuleFor(x => x.CompanyCountry).NotNull().NotEmpty();
            this.RuleFor(x => x.CompanyCity).NotNull().NotEmpty();
            this.RuleFor(x => x.CompanyRegion).NotNull().NotEmpty();
            this.RuleFor(x => x.CompanyPostalCode).NotNull().NotEmpty();
        }
    }
}
