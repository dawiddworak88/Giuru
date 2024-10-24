﻿using FluentValidation;
using Foundation.Extensions.Validators;
using Global.Api.ServicesModels.Countries;

namespace Global.Api.validators.Countries
{
    public class CreateCountryModelValidator : BaseServiceModelValidator<CreateCountryServiceModel>
    {
        public CreateCountryModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
