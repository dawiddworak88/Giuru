using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Dashboard.ViewModels;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Dashboard.ModelBuilders
{
    public class DashboardPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DashboardPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> _headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DashboardDetailViewModel> _dashboardDetailModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> _menuTilesModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;

        public DashboardPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, DashboardDetailViewModel> dashboardDetailModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder) 
        {
            _headerModelBuilder = headerModelBuilder;
            _menuTilesModelBuilder = menuTilesModelBuilder;
            _dashboardDetailModelBuilder = dashboardDetailModelBuilder;
            _footerModelBuilder = footerModelBuilder;
        }

        public async Task<DashboardPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DashboardPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = _menuTilesModelBuilder.BuildModel(),
                DashboardDetail = await _dashboardDetailModelBuilder.BuildModelAsync(componentModel),
                Footer = _footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
