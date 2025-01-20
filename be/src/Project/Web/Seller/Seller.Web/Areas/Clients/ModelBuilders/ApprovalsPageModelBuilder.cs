using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ApprovalsPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ApprovalsPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> _headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> _menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Approval>> _approvalsModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;

        public ApprovalsPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Approval>> approvalsModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _menuTilesModelBuilder = menuTilesModelBuilder;
            _approvalsModelBuilder = approvalsModelBuilder;
            _footerModelBuilder = footerModelBuilder;
        }

        public async Task<ApprovalsPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ApprovalsPageViewModel
            {
                Locale = CultureInfo.CurrentCulture.Name,
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = _menuTilesModelBuilder.BuildModel(),
                Catalog = await _approvalsModelBuilder.BuildModelAsync(componentModel),
                Footer = _footerModelBuilder.BuildModel(),
            };

            return viewModel;
        }
    }
}
