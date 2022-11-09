using Seller.Web.Areas.Orders.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using System.Globalization;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class BasketPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, BasketPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BasketFormViewModel> basketFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public BasketPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BasketFormViewModel> basketFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.basketFormModelBuilder = basketFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<BasketPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new BasketPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                BasketForm = await this.basketFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
