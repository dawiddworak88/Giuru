﻿using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Shared.ViewModels.Base;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;

namespace Buyer.Web.Areas.Orders.ViewModel
{
    public class OrdersPageViewModel : BasePageViewModel
    {
        public MetadataViewModel Metadata { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public CatalogOrderViewModel<Order> Catalog { get; set; }
    }
}
