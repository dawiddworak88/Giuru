using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.v1.Areas.Clients.Models;

namespace Identity.Api.v1.Areas.Clients.Validators
{
    public class GetClientModelValidator : BaseServiceModelValidator<GetClientModel>
    {
        public GetClientModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
