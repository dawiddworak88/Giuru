using Client.Api.ServicesModels.Applications;
using Foundation.Extensions.Validators;
using FluentValidation;

namespace Client.Api.Validators.Applications
{
    public class ClientApplicationAddressModelValidator : BaseServiceModelValidator<ClientApplicationAddressServiceModel>
    {
        public ClientApplicationAddressModelValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().NotNull();
            RuleFor(x => x.PhoneNumber).NotEmpty().NotNull();
            RuleFor(x => x.Street).NotEmpty().NotNull();
            RuleFor(x => x.Region).NotEmpty().NotNull();
            RuleFor(x => x.PostalCode).NotEmpty().NotNull();
            RuleFor(x => x.City).NotEmpty().NotNull();
            RuleFor(x => x.Country).NotEmpty().NotNull();
        }
    }
}
