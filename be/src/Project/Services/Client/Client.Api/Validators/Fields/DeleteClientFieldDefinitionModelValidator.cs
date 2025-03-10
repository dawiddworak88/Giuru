﻿using Client.Api.ServicesModels.Fields;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Fields
{
    public class DeleteClientFieldDefinitionModelValidator : BaseServiceModelValidator<DeleteClientFieldServiceModel>
    {
        public DeleteClientFieldDefinitionModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
