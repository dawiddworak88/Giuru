using Client.Api.ServicesModels.Managers;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Managers
{
    public class DeleteClientManagerModelValidator : BaseServiceModelValidator<DeleteClientManagerServiceModel>
    {
        public DeleteClientManagerModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
