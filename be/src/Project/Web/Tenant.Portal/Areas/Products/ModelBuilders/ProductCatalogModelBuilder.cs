using Feature.Product.Resources;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using Tenant.Portal.Areas.Products.ViewModels;

namespace Tenant.Portal.Areas.Products.ModelBuilders
{
    public class ProductCatalogModelBuilder : IModelBuilder<ProductCatalogViewModel>
    {
        private readonly IStringLocalizer<ProductResources> orderLocalizer;

        public ProductCatalogModelBuilder(IStringLocalizer<ProductResources> orderLocalizer)
        {
            this.orderLocalizer = orderLocalizer;
        }

        public ProductCatalogViewModel BuildModel()
        {
            return new ProductCatalogViewModel
            {
                Title = this.orderLocalizer["Products"],
                ShowNew = true,
                NewText = this.orderLocalizer["NewProduct"],
                NewUrl = "#"
            };
        }
    }
}
