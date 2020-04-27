using Feature.Client.Models;
using FluentValidation;
using System;

namespace Feature.Client.Validators
{
    public class CreateClientModelValidator : AbstractValidator<CreateClientModel>
    {
        public CreateClientModelValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.TenantId).NotNull().NotEqual(Guid.Empty);
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.ClientPreferredLanguage).NotNull().NotEmpty();
        }
    }
}
