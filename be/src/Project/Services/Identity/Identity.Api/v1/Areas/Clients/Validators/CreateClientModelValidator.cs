using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.v1.Areas.Clients.Models;

namespace Identity.Api.v1.Areas.Clients.Validators
{
    public class CreateClientModelValidator : BaseServiceModelValidator<CreateClientModel>
    {
        public CreateClientModelValidator()
        {
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}