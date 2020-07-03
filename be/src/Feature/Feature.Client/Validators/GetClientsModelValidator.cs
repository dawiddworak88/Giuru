using Feature.Client.Models;
using Foundation.Extensions.Validators;
using FluentValidation;

namespace Feature.Client.Validators
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
