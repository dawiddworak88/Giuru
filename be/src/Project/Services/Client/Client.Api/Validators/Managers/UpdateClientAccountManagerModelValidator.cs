using Client.Api.ServicesModels.Managers;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Managers
{
    public class UpdateClientAccountManagerModelValidator : BaseServiceModelValidator<UpdateClientAccountManagerServiceModel>
    {
        public UpdateClientAccountManagerModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
            this.RuleFor(x => x.FirstName).NotEmpty().NotNull();
            this.RuleFor(x => x.LastName).NotEmpty().NotNull();
            this.RuleFor(x => x.Email).NotEmpty().NotNull();
        }
    }
}
