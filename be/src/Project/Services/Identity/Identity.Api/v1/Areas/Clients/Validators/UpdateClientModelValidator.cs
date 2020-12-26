using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.v1.Areas.Clients.Models;

namespace Identity.Api.v1.Areas.Clients.Validators
{
    public class UpdateClientModelValidator : BaseServiceModelValidator<UpdateClientModel>
    {
        public UpdateClientModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
