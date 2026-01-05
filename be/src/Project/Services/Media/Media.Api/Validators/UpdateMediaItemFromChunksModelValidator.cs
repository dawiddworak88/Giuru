using FluentValidation;
using Foundation.Extensions.Validators;
using Media.Api.ServicesModels;
using System;

namespace Media.Api.Validators
{
    public class UpdateMediaItemFromChunksModelValidator : BaseAuthorizedServiceModelValidator<UpdateMediaItemFromChunksServiceModel>
    {
        public UpdateMediaItemFromChunksModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.UploadId).NotEqual(Guid.Empty);
            this.RuleFor(x => x.Filename).NotNull().NotEmpty();
            this.RuleFor(x => x.OrganisationId).NotNull().NotEmpty();
        }
    }
}
