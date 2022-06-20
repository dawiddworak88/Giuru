using Client.Api.ServicesModels.Managers;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Managers
{
    public class GetClientAccountManagerModelValidator : BaseServiceModelValidator<GetClientAccountManagerServiceModel>
    {
        public GetClientAccountManagerModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
