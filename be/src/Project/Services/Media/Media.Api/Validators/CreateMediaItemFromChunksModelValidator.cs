using FluentValidation;
using Foundation.Extensions.Validators;
using Media.Api.ServicesModels;
using System;

namespace Media.Api.Validators
{
    public class CreateMediaItemFromChunksModelValidator : BaseAuthorizedServiceModelValidator<CreateMediaItemFromChunksServiceModel>
    {
        public CreateMediaItemFromChunksModelValidator()
        {
            this.RuleFor(x => x.UploadId).NotEqual(Guid.Empty);
            this.RuleFor(x => x.Filename).NotNull().NotEmpty();
        }
    }
}
