using Client.Api.ServicesModels.Addresses;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Addresses
{
    public class DeleteClientAddressModelValidator : BaseServiceModelValidator<DeleteClientAddressServiceModel>
    {
        public DeleteClientAddressModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
