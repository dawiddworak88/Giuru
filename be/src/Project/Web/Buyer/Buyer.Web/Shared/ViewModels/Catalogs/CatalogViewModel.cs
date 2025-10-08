using Buyer.Web.Shared.DomainModels.Baskets;
using Buyer.Web.Shared.ViewModels.Filters;
using Buyer.Web.Shared.ViewModels.Modals;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Buyer.Web.Shared.ViewModels.Toasts;
using Foundation.GenericRepository.Paginations;
using Foundation.Search.Models;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Catalogs
{
    public class CatalogViewModel
    {
        public Guid? BasketId { get; set; }
        public int? ItemsPerPage { get; set; }
        public string Title { get; set; }
        public string ResultsLabel { get; set; }
        public string NoResultsLabel { get; set; }
        public string SkuLabel { get; set; }
        public string ByLabel { get; set; }
        public string InStockLabel { get; set; }
        public string InOutletLabel { get; set; }
        public string BasketLabel { get; set; }
        public string PrimaryFabricLabel { get; set; }
        public bool ShowAddToCartButton { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool IsDefaultOutletOrder { get; set; }
        public string SignInUrl { get; set; }
        public string SignInToSeePricesLabel { get; set; }
        public string DisplayedRowsLabel { get; set; }
        public string RowsPerPageLabel { get; set; }
        public string BackIconButtonText { get; set; }
        public string NextIconButtonText { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string QuantityErrorMessage { get; set; }
        public string ProductsApiUrl { get; set; }
        public string UpdateBasketUrl { get; set; }
        public string ExpectedDeliveryLabel { get; set; }
        public FiltersCollectorViewModel FilterCollector { get; set; }
        public QueryFilters Filters { get; set; }
        public string GetProductPriceUrl { get; set; }
        public int? MaxAllowedOrderQuantity { get; set; }
        public string MaxAllowedOrderQuantityErrorMessage { get; set; }
        public SuccessAddProductToBasketViewModel ToastSuccessAddProductToBasket { get; set; }
        public string MinOrderQuantityErrorMessage { get; set; }
        public SidebarViewModel Sidebar { get; set; }
        public ModalViewModel Modal { get; set; }
        public IEnumerable<BasketItem> BasketItems { get; set; }
        public PagedResults<IEnumerable<CatalogItemViewModel>> PagedItems { get; set; }
    }
}
