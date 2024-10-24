﻿using Client.Api.ServicesModels.Roles;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Roles
{
    public class GetClientRolesByIdsModelValidator : BaseServiceModelValidator<GetClientRolesByIdsServiceModel>
    {
        public GetClientRolesByIdsModelValidator()
        {
            this.RuleFor(x => x.Ids).NotEmpty().NotNull();
        }
    }
}
