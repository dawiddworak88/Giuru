using Buyer.Web.Shared.Services.Baskets;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Buyer.Web.Shared.ModelBuilders.Catalogs
{
    public class CatalogModelBuilder<S, T> : ICatalogModelBuilder<S, T> where S: ComponentModelBase where T: CatalogViewModel, new()
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IBasketService basketService;

        public CatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            IBasketService basketService,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
            this.basketService = basketService;
        }

        public T BuildModel(S componentModel)
        {
            var viewModel = new T
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
                IsLoggedIn = componentModel.IsAuthenticated,
                BasketId = componentModel.BasketId,
                SuccessfullyAddedProduct = this.globalLocalizer.GetString("SuccessfullyAddedProduct"),
                ProductsApiUrl = this.linkGenerator.GetPathByAction("Get", "ProductsApi", new { Area = "Products" }),
                UpdateBasketUrl = this.linkGenerator.GetPathByAction("Index", "BasketsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.IsAuthenticated)
            {
                var basketItems = this.basketService.GetBasketAsync(componentModel.BasketId, componentModel.Token, componentModel.Language).Result;
                if (basketItems is not null)
                {
                    viewModel.OrderItems = basketItems;
                }
            }

            return viewModel;
        }
    }
}
