﻿using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels.OrderAttributeOptions;

namespace Ordering.Api.Validators.OrderAttributeOptions
{
    public class GetOrderAttributeOptionModelValidator : BaseServiceModelValidator<GetOrderAttributeOptionServiceModel>
    {
        public GetOrderAttributeOptionModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}