using Client.Api.ServicesModels.Managers;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Managers
{
    public class UpdateClientManagerModelValidator : BaseServiceModelValidator<UpdateClientManagerServiceModel>
    {
        public UpdateClientManagerModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
            this.RuleFor(x => x.FirstName).NotEmpty().NotNull();
            this.RuleFor(x => x.LastName).NotEmpty().NotNull();
            this.RuleFor(x => x.Email).NotEmpty().NotNull();
        }
    }
}
