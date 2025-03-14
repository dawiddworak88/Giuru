﻿using Catalog.Api.ServicesModels.Categories;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Categories
{
    public class GetCategorySchemasModelValidator : BaseServiceModelValidator<GetCategorySchemasServiceModel>
    {
        public GetCategorySchemasModelValidator() 
        { 
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
