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

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class OrderItemPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderItemPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> _seoModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> _headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> _mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderItemFormViewModel> _orderItemFormModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> _footerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> _notificationBarModelBuilder;

        public OrderItemPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, OrderItemFormViewModel> orderItemFormModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> notificationBarModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _mainNavigationModelBuilder = mainNavigationModelBuilder;
            _orderItemFormModelBuilder = orderItemFormModelBuilder;
            _footerModelBuilder = footerModelBuilder;
            _seoModelBuilder = seoModelBuilder;
            _notificationBarModelBuilder = notificationBarModelBuilder;
        }

        public async Task<OrderItemPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderItemPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Metadata = await _seoModelBuilder.BuildModelAsync(componentModel),
                NotificationBar = await _notificationBarModelBuilder.BuildModelAsync(componentModel),
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await _mainNavigationModelBuilder.BuildModelAsync(componentModel),
                OrderItemForm = await _orderItemFormModelBuilder.BuildModelAsync(componentModel),
                Footer = await _footerModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
