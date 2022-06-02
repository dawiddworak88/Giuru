using Client.Api.ServicesModels.Applications;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Applications
{
    public class DeleteClientApplicationModelValidator : BaseServiceModelValidator<DeleteClientApplicationServiceModel>
    {
        public DeleteClientApplicationModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
