using Feature.Client.Models;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Feature.Client.Validators
{
    public class CreateClientModelValidator : BaseServiceModelValidator<CreateClientModel>
    {
        public CreateClientModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.ClientPreferredLanguage).NotNull().NotEmpty();
        }
    }
}
