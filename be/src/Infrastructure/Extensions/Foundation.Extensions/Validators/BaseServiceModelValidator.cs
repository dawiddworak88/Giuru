using FluentValidation;
using Foundation.Extensions.Models;

namespace Foundation.Extensions.Validators
{
    public class BaseServiceModelValidator<T> : AbstractValidator<T> where T : BaseServiceModel
    {
        public BaseServiceModelValidator()
        {
        }
    }
}
