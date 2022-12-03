using Client.Api.ServicesModels.Applications;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Applications
{
    public class GetClientApplicationModelValidator : BaseServiceModelValidator<GetClientApplicationServiceModel>
    {
        public GetClientApplicationModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
