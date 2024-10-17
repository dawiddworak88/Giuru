using Client.Api.ServicesModels.Clients;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Clients
{
    public class GetClientBySellerIdModelValiodator : BaseServiceModelValidator<GetClientBySellerIdServiceModel>
    {
        public GetClientBySellerIdModelValiodator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
