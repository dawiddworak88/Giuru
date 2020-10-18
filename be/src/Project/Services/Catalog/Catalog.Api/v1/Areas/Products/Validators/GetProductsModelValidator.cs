using Catalog.Api.v1.Areas.Products.Models;
using FluentValidation;
using Foundation.Extensions.Validators;
using System;

namespace Catalog.Api.v1.Areas.Products.Validators
{
    public class GetProductsModelValidator : BaseServiceModelValidator<GetProductsModel>
    {
        public GetProductsModelValidator()
        {
            RuleFor(m => new { m.CategoryId, m.OrganisationId }).Must(x => ValidCategoryOrOrganisationId(x.CategoryId, x.OrganisationId));
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(1);
            RuleFor(x => x.ItemsPerPage).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
        }

        private bool ValidCategoryOrOrganisationId(Guid? categoryId, Guid? organisationId)
        {
            if (categoryId.HasValue || organisationId.HasValue)
            {
                return true;
            }

            return false;
        }
    }
}
