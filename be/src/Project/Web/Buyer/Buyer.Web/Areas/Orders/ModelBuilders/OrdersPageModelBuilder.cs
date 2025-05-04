using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.ViewModel;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using Buyer.Web.Shared.ViewModels.NotificationBar;
using System.Globalization;
using System.Threading.Tasks;
using Buyer.Web.Areas.Orders.ComponentModels;
using System;

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class OrdersPageModelBuilder : IAsyncComponentModelBuilder<OrdersPageComponentModel, OrdersPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> _seoModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> _headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<OrdersPageComponentModel, CatalogOrderViewModel<Order>> _ordersCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> _mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> _footerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> _notificationBarModelBuilder;

        public OrdersPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<OrdersPageComponentModel, CatalogOrderViewModel<Order>> ordersCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> notificationBarModelBuilder)
        {
            _seoModelBuilder = seoModelBuilder;
            _headerModelBuilder = headerModelBuilder;
            _ordersCatalogModelBuilder = ordersCatalogModelBuilder;
            _footerModelBuilder = footerModelBuilder;
            _mainNavigationModelBuilder = mainNavigationModelBuilder;
            _notificationBarModelBuilder = notificationBarModelBuilder;
        }

        public async Task<OrdersPageViewModel> BuildModelAsync(OrdersPageComponentModel componentModel)
        {
            var viewModel = new OrdersPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Metadata = await _seoModelBuilder.BuildModelAsync(componentModel),
                NotificationBar = await _notificationBarModelBuilder.BuildModelAsync(componentModel),
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await _mainNavigationModelBuilder.BuildModelAsync(componentModel),
                Catalog = await _ordersCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = await _footerModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
