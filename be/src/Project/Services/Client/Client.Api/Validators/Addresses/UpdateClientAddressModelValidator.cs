using Client.Api.ServicesModels.Addresses;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Addresses
{
    public class UpdateClientAddressModelValidator : BaseServiceModelValidator<UpdateClientAddressServiceModel>
    {
        public UpdateClientAddressModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
            RuleFor(x => x.ClientId).NotNull().NotEmpty();
            RuleFor(x => x.CountryId).NotNull().NotEmpty();
            RuleFor(x => x.PostCode).NotNull().NotEmpty();
            RuleFor(x => x.Street).NotNull().NotEmpty();
            RuleFor(x => x.Region).NotNull().NotEmpty();
            RuleFor(x => x.City).NotNull().NotEmpty();
        }
    }
}
