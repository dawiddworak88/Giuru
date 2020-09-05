using Buyer.Web.Areas.Products.ViewModels.Categories;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Categories
{
    public class CategoryCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoryCatalogViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;

        public CategoryCatalogModelBuilder(IStringLocalizer<GlobalResources> globalLocalizer, IStringLocalizer<ProductResources> productLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
        }

        public async Task<CategoryCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CategoryCatalogViewModel
            {
                SkuLabel = this.productLocalizer.GetString("Sku"),
                SignInUrl = "#",
                SignInToSeePricesLabel = this.globalLocalizer.GetString("SignInToSeePrices"),
                ResultsLabel = this.globalLocalizer.GetString("Results"),
                ByLabel = this.globalLocalizer.GetString("By"),
                InStockLabel = this.productLocalizer.GetString("InStock"),
                NoResultsLabel = this.globalLocalizer.GetString("NoResults"),
                IsAuthenticated = componentModel.IsAuthenticated,
            };

            return viewModel;
        }
    }
}
