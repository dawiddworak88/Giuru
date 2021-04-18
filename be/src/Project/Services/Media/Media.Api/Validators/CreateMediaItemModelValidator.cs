using FluentValidation;
using Foundation.Extensions.Validators;
using Media.Api.ServicesModels;

namespace Media.Api.Validators
{
    public class CreateMediaItemModelValidator : BaseAuthorizedServiceModelValidator<CreateMediaItemServiceModel>
    {
        public CreateMediaItemModelValidator()
        {
            this.RuleFor(x => x.File).NotNull();
        }
    }
}
