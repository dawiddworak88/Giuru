﻿using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Inventory.ViewModel
{
    public class InventoriesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<InventoryItem> Catalog { get; set; }
    }
}
