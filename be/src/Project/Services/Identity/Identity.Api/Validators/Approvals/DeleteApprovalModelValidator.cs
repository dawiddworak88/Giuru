﻿using FluentValidation;
using Foundation.Extensions.Validators;
using Identity.Api.ServicesModels.Approvals;

namespace Identity.Api.Validators.Approvals
{
    public class DeleteApprovalModelValidator : BaseServiceModelValidator<DeleteApprovalServiceModel>
    {
        public DeleteApprovalModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
