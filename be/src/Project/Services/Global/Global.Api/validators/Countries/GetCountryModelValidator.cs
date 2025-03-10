﻿using FluentValidation;
using Foundation.Extensions.Validators;
using Global.Api.ServicesModels.Countries;

namespace Global.Api.validators.Countries
{
    public class GetCountryModelValidator : BaseServiceModelValidator<GetCountryServiceModel>
    {
        public GetCountryModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
