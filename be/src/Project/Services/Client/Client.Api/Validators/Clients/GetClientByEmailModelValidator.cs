using Client.Api.ServicesModels.Clients;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Clients
{
    public class GetClientByEmailModelValidator : BaseServiceModelValidator<GetClientByEmailServiceModel>
    {
        public GetClientByEmailModelValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty();
        }
    }
}
