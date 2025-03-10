﻿using Catalog.Api.ServicesModels.Categories;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Categories
{
    public class UpdateCategorySchemaModelValidator : BaseAuthorizedServiceModelValidator<UpdateCategorySchemaServiceModel>
    {
        public UpdateCategorySchemaModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
            RuleFor(x => x.Schema).NotNull().NotEmpty();
        }
    }
}
