using FluentValidation;
using Foundation.Extensions.Models;

namespace Foundation.Extensions.Validators
{
    public class BaseAuthorizedServiceModelValidator<T> : AbstractValidator<T> where T: BaseServiceModel
    {
        public BaseAuthorizedServiceModelValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.OrganisationId).NotNull();
        }
    }
}
