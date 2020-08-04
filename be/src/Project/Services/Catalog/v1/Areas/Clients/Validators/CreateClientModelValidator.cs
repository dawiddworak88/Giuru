using Catalog.Api.v1.Areas.Clients.Models;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.v1.Areas.Clients.Validators
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
