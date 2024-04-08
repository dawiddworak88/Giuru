using Buyer.Web.Areas.Content.ComponentModels;
using Buyer.Web.Areas.Content.ViewModel;
using Buyer.Web.Shared.ViewModels.Headers;
using Buyer.Web.Shared.ViewModels.NotificationBar;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Content.ModelBuilders
{
    public class SlugPageModelBuilder : IAsyncComponentModelBuilder<SlugContentComponentModel, SlugPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> _headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> _mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> _footerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> _notificationBarModelBuilder;

        public SlugPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel> notificationBarModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _footerModelBuilder = footerModelBuilder;
            _mainNavigationModelBuilder = mainNavigationModelBuilder;
            _notificationBarModelBuilder = notificationBarModelBuilder;
        }

        public async Task<SlugPageViewModel> BuildModelAsync(SlugContentComponentModel componentModel)
        {
            var viewModel = new SlugPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                NotificationBar = await _notificationBarModelBuilder.BuildModelAsync(componentModel),
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await _mainNavigationModelBuilder.BuildModelAsync(componentModel),
                Footer = await _footerModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
