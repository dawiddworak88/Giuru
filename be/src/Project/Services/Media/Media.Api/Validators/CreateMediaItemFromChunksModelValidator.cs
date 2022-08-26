using FluentValidation;
using Foundation.Extensions.Validators;
using Media.Api.ServicesModels;

namespace Media.Api.Validators
{
    public class CreateMediaItemFromChunksModelValidator : BaseAuthorizedServiceModelValidator<CreateMediaItemFromChunksServiceModel>
    {
        public CreateMediaItemFromChunksModelValidator()
        {
            this.RuleFor(x => x.Filename).NotNull().NotEmpty();
        }
    }
}
