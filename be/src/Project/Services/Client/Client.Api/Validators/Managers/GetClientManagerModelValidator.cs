using Client.Api.ServicesModels.Managers;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Managers
{
    public class GetClientManagerModelValidator : BaseServiceModelValidator<GetClientManagerServiceModel>
    {
        public GetClientManagerModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
