using Client.Api.ServicesModels.Applications;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Applications
{
    public class CreateClientApplicationModelValidator : BaseServiceModelValidator<CreateClientApplicationServiceModel>
    {
        public CreateClientApplicationModelValidator()
        {
            RuleFor(x => x.CompanyName).NotEmpty().NotNull();
            RuleFor(x => x.Email).NotEmpty().NotNull();
            RuleFor(x => x.FirstName).NotEmpty().NotNull();
            RuleFor(x => x.LastName).NotEmpty().NotNull();
            RuleFor(x => x.PhoneNumber).NotEmpty().NotNull();
            RuleFor(x => x.ContactJobTitle).NotEmpty().NotNull();
            RuleFor(x => x.BillingAddress).SetValidator(new ClientApplicationAddressModelValidator());
            RuleFor(x => x.DeliveryAddress).SetValidator(new ClientApplicationAddressModelValidator()).When(x => x.IsDeliveryAddressEqualBillingAddress is false);
        }
    }
}
