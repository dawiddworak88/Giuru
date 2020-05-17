using Feature.Product.Resources;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Tenant.Portal.Areas.Products.ViewModels;

namespace Tenant.Portal.Areas.Products.ModelBuilders
{
    public class ProductCatalogModelBuilder : IModelBuilder<ProductCatalogViewModel>
    {
        private readonly IStringLocalizer<ProductResources> orderLocalizer;

        private readonly LinkGenerator linkGenerator;

        public ProductCatalogModelBuilder(
            IStringLocalizer<ProductResources> orderLocalizer,
            LinkGenerator linkGenerator)
        {
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public ProductCatalogViewModel BuildModel()
        {
            return new ProductCatalogViewModel
            {
                Title = this.orderLocalizer["Products"],
                ShowNew = true,
                NewText = this.orderLocalizer["NewProduct"],
                NewUrl = this.linkGenerator.GetPathByAction("Index", "ProductDetail", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
            };
        }
    }
}
