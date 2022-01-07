using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;

namespace Buyer.Web.Shared.ModelBuilders.Catalogs
{
    public class CatalogModelBuilder<S, T> : ICatalogModelBuilder<S, T> where S: ComponentModelBase where T: CatalogViewModel, new()
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public CatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public T BuildModel(S componentModel)
        {
            return new T
            {
                SkuLabel = this.productLocalizer.GetString("Sku"),
                SignInUrl = "#",
                SignInToSeePricesLabel = this.globalLocalizer.GetString("SignInToSeePrices"),
                ResultsLabel = this.globalLocalizer.GetString("Results"),
                ByLabel = this.globalLocalizer.GetString("By"),
                InStockLabel = this.globalLocalizer.GetString("InStock"),
                BasketLabel = this.globalLocalizer.GetString("BasketLabel"),
                PrimaryFabricLabel = this.globalLocalizer.GetString("PrimaryFabricLabel"),
                NoResultsLabel = this.globalLocalizer.GetString("NoResults"),
                GeneralErrorMessage = this.globalLocalizer["AnErrorOccurred"],
                DisplayedRowsLabel = this.globalLocalizer["DisplayedRows"],
                RowsPerPageLabel = this.globalLocalizer["RowsPerPage"],
                BackIconButtonText = this.globalLocalizer["Previous"],
                NextIconButtonText = this.globalLocalizer["Next"],
                IsAuthenticated = componentModel.IsAuthenticated,
                ProductsApiUrl = this.linkGenerator.GetPathByAction("Get", "ProductsApi", new { Area = "Products" })
            };
        }
    }
}
