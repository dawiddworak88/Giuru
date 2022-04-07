using Buyer.Web.Shared.DomainModels.Baskets;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Foundation.GenericRepository.Paginations;
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
        public string BasketLabel { get; set; }
        public string PrimaryFabricLabel { get; set; }
        public bool ShowBrand { get; set; }
        public bool ShowAddToCartButton { get; set; }
        public bool IsLoggedIn { get; set; }
        public string SignInUrl { get; set; }
        public string SignInToSeePricesLabel { get; set; }
        public string DisplayedRowsLabel { get; set; }
        public string RowsPerPageLabel { get; set; }
        public string BackIconButtonText { get; set; }
        public string NextIconButtonText { get; set; }
        public string SuccessfullyAddedProduct { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string ProductsApiUrl { get; set; }
        public string UpdateBasketUrl { get; set; }
        public string ExpectedDeliveryLabel { get; set; }
        public SidebarViewModel Sidebar { get; set; }
        public IEnumerable<BasketItem> BasketItems { get; set; }
        public PagedResults<IEnumerable<CatalogItemViewModel>> PagedItems { get; set; }
    }
}
