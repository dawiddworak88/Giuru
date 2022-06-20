using FluentValidation;
using Foundation.Extensions.Validators;
using Media.Api.ServicesModels;

namespace Media.Api.Validators
{
    public class UpdateMediaItemVersionModelValidator : BaseServiceModelValidator<UpdateMediaItemVersionServiceModel>
    {
        public UpdateMediaItemVersionModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
