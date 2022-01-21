using FluentValidation;
using Identity.Api.Areas.Accounts.ApiRequestModels;

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
