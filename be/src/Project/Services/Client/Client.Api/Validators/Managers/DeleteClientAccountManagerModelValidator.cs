using Client.Api.ServicesModels.Managers;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Managers
{
    public class DeleteClientAccountManagerModelValidator : BaseServiceModelValidator<DeleteClientAccountManagerServiceModel>
    {
        public DeleteClientAccountManagerModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
