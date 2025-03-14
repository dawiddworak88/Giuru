﻿using DownloadCenter.Api.ServicesModels.Categories;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace DownloadCenter.Api.Validators.Categories
{
    public class UpdateCategoryModelValidator : BaseServiceModelValidator<UpdateCategoryServiceModel>
    {
        public UpdateCategoryModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
            this.RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
