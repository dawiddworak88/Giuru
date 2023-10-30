using Client.Api.ServicesModels.Addresses;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Addresses
{
    public class GetClientAddressModelValidator : BaseServiceModelValidator<GetClientAddressServiceModel>
    {
        public GetClientAddressModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
