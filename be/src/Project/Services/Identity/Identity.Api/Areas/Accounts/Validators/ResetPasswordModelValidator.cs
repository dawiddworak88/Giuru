using FluentValidation;
using Identity.Api.Areas.Accounts.ApiRequestModels;

namespace Identity.Api.Areas.Accounts.Validators
{
    public class ResetPasswordModelValidator : AbstractValidator<ResetUserPasswordRequestModel>
    {
        public ResetPasswordModelValidator()
        {
            this.RuleFor(x => x.Email).NotNull().NotEmpty();
        }
    }
}
