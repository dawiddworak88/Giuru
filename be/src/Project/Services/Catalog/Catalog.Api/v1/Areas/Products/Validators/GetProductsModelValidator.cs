using Catalog.Api.v1.Areas.Products.Models;
using FluentValidation;
using Foundation.Extensions.Validators;
using System;

namespace Catalog.Api.v1.Areas.Products.Validators
{
    public class GetProductsModelValidator : BasePagedServiceModelValidator<GetProductsModel>
    {
        public GetProductsModelValidator()
        {
            RuleFor(m => new { m.CategoryId, m.OrganisationId, m.SearchTerm }).Must(x => ValidCategoryOrOrganisationId(x.CategoryId, x.OrganisationId, x.SearchTerm));
        }

        private bool ValidCategoryOrOrganisationId(Guid? categoryId, Guid? organisationId, string searchTerm)
        {
            if (categoryId.HasValue || organisationId.HasValue || !string.IsNullOrWhiteSpace(searchTerm))
            {
                return true;
            }

            return false;
        }
    }
}
