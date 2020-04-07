using Client.Api.v1.RequestModels;
using FluentValidation;

namespace Client.Api.v1.Validators
{
    public class ClientRequestModelValidator : AbstractValidator<ClientRequestModel>
    {
        public ClientRequestModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Language).NotNull().NotEmpty();
        }
    }
}
