using FluentValidation;
using Identity.Api.Areas.Accounts.ApiRequestModels;
using Identity.Api.Areas.Accounts.Models;

namespace Identity.Api.Areas.Accounts.Validators
{
    public class SetPasswordModelValidator : AbstractValidator<SetUserPasswordRequestModel>
    {
        public SetPasswordModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(8);
        }
    }
}
