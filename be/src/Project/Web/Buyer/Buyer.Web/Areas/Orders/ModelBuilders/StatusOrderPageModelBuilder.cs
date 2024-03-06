using Buyer.Web.Areas.Orders.ViewModel;
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

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class StatusOrderPageModelBuilder : IAsyncComponentModelBuilder<OrdersPageComponentModel, StatusOrderPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> _seoModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> _headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> _mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<OrdersPageComponentModel, StatusOrderFormViewModel> _editOrderFormModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> _footerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> _notificationBarModelBuilder;

        public StatusOrderPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<OrdersPageComponentModel, StatusOrderFormViewModel> editOrderFormModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> notificationBarModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _editOrderFormModelBuilder = editOrderFormModelBuilder;
            _footerModelBuilder = footerModelBuilder;
            _mainNavigationModelBuilder = mainNavigationModelBuilder;
            _seoModelBuilder = seoModelBuilder;
            _notificationBarModelBuilder = notificationBarModelBuilder;
        }

        public async Task<StatusOrderPageViewModel> BuildModelAsync(OrdersPageComponentModel componentModel)
        {
            var viewModel = new StatusOrderPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Metadata = await _seoModelBuilder.BuildModelAsync(componentModel),
                NotificationBar = await _notificationBarModelBuilder.BuildModelAsync(componentModel),
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await _mainNavigationModelBuilder.BuildModelAsync(componentModel),
                StatusOrder = await _editOrderFormModelBuilder.BuildModelAsync(componentModel),
                Footer = await _footerModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
