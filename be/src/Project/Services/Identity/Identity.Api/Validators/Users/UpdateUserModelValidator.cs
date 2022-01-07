using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Users;

namespace Identity.Api.Validators.Users
{
    public class UpdateUserModelValidator : BaseServiceModelValidator<UpdateUserServiceModel>
    {
        public UpdateUserModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
