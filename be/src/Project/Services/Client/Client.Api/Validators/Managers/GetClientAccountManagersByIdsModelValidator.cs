using Client.Api.ServicesModels.Managers;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Managers
{
    public class GetClientAccountManagersByIdsModelValidator : BaseServiceModelValidator<GetClientAccountManagersByIdsServiceModel>
    {
        public GetClientAccountManagersByIdsModelValidator()
        {
            this.RuleFor(x => x.Ids).NotEmpty().NotNull();
        }
    }
}
