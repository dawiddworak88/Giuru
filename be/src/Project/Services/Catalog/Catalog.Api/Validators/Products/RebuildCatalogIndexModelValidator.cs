using Catalog.Api.ServicesModels.Products;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Catalog.Api.Validators.Products
{
    public class RebuildCatalogIndexModelValidator : BaseServiceModelValidator<RebuildCatalogIndexServiceModel>
    {
        public RebuildCatalogIndexModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotNull().NotEmpty();
        }
    }
}
