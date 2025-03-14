﻿using DownloadCenter.Api.ServicesModels.Categories;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace DownloadCenter.Api.Validators.Categories
{
    public class DeleteCategoryModelValidator : BaseServiceModelValidator<DeleteCategoryServiceModel>
    {
        public DeleteCategoryModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
