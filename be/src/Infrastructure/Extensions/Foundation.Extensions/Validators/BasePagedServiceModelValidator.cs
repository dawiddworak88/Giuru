using FluentValidation;
using Foundation.Extensions.Models;

namespace Foundation.Extensions.Validators
{
    public class BasePagedServiceModelValidator<T> : BaseServiceModelValidator<T> where T: PagedBaseServiceModel
    {
        public BasePagedServiceModelValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(1);
            RuleFor(x => x.ItemsPerPage).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
        }
    }
}
