using Client.Api.ServicesModels.Addresses;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Addresses
{
    public class GetClientAddressesByIdsModelValidator : BaseServiceModelValidator<GetClientAddressesByIdsServiceModel>
    {
        public GetClientAddressesByIdsModelValidator()
        {
            RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
