﻿using FluentValidation;
using Foundation.Extensions.Validators;
using Media.Api.ServicesModels;

namespace Media.Api.Validators
{
    public class CreateFileChunkModelValidator : BaseAuthorizedServiceModelValidator<CreateFileChunkServiceModel>
    {
        public CreateFileChunkModelValidator()
        {
            this.RuleFor(x => x.File).NotNull();
            this.RuleFor(x => x.ChunkSumber).NotNull().NotEmpty();
        }
    }
}
