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
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;
        private readonly IStringLocalizer<InventoryResources> _inventoryLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly IBasketService _basketService;

        public CatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            IStringLocalizer<InventoryResources> inventoryLocalizer,
            IBasketService basketService,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _productLocalizer = productLocalizer;
            _linkGenerator = linkGenerator;
            _basketService = basketService;
            _inventoryLocalizer = inventoryLocalizer;
        }

        public T BuildModel(S componentModel)
        {
            var viewModel = new T
            {
                SkuLabel = _productLocalizer.GetString("Sku"),
                SignInUrl = "#",
                SignInToSeePricesLabel = _globalLocalizer.GetString("SignInToSeePrices"),
                ResultsLabel = _globalLocalizer.GetString("Results"),
                ByLabel = _globalLocalizer.GetString("By"),
                InStockLabel = _globalLocalizer.GetString("InStock"),
                InOutletLabel = _globalLocalizer.GetString("InOutlet"),
                BasketLabel = _globalLocalizer.GetString("BasketLabel"),
                PrimaryFabricLabel = _globalLocalizer.GetString("PrimaryFabricLabel"),
                NoResultsLabel = _globalLocalizer.GetString("NoResults"),
                GeneralErrorMessage = _globalLocalizer["AnErrorOccurred"],
                DisplayedRowsLabel = _globalLocalizer["DisplayedRows"],
                RowsPerPageLabel = _globalLocalizer["RowsPerPage"],
                IsLoggedIn = componentModel.IsAuthenticated,
                BasketId = componentModel.BasketId,
                SuccessfullyAddedProduct = _globalLocalizer.GetString("SuccessfullyAddedProduct"),
                QuantityErrorMessage = _globalLocalizer.GetString("QuantityErrorMessage"),
                ProductsApiUrl = _linkGenerator.GetPathByAction("Get", "ProductsApi", new { Area = "Products" }),
                UpdateBasketUrl = _linkGenerator.GetPathByAction("Index", "BasketsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                ExpectedDeliveryLabel = _inventoryLocalizer.GetString("ExpectedDeliveryLabel"),
            };

            if (componentModel.IsAuthenticated && componentModel.BasketId.HasValue)
            {
                var basketItems = _basketService.GetBasketAsync(componentModel.BasketId, componentModel.Token, componentModel.Language).Result;

                if (basketItems is not null)
                {
                    viewModel.BasketItems = basketItems;
                }
            }

            return viewModel;
        }
    }
}
