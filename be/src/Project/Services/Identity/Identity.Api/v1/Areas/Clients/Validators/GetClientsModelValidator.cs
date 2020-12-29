using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.v1.Areas.Clients.Models;

namespace Identity.Api.v1.Areas.Clients.Validators
{
    public class GetClientsModelValidator : BasePagedServiceModelValidator<GetClientsModel>
    {
        public GetClientsModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotNull().NotEmpty();
        }
    }
}
