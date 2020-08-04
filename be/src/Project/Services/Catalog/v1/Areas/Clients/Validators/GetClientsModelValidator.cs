using Catalog.Api.v1.Areas.Clients.Models;
using Foundation.Extensions.Validators;
using FluentValidation;

namespace Catalog.Api.v1.Areas.Clients.Validators
{
    public class GetClientsModelValidator : BaseServiceModelValidator<GetClientsModel>
    {
        public GetClientsModelValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(1);
            RuleFor(x => x.ItemsPerPage).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
        }
    }
}
