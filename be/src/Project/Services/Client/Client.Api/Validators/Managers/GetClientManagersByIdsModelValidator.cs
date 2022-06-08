using Client.Api.ServicesModels.Managers;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Managers
{
    public class GetClientManagersByIdsModelValidator : BasePagedServiceModelValidator<GetClientManagersByIdsServiceModel>
    {
        public GetClientManagersByIdsModelValidator()
        {
            this.RuleFor(x => x.Ids).NotEmpty().NotNull();
        }
    }
}
