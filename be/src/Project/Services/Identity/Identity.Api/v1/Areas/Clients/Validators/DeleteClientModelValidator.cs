using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.v1.Areas.Clients.Models;

namespace Identity.Api.v1.Areas.Clients.Validators
{
    public class DeleteClientModelValidator : BaseServiceModelValidator<DeleteClientModel>
    {
        public DeleteClientModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull();
        }
    }
}
