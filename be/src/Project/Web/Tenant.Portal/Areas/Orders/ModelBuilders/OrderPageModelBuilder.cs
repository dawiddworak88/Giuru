using Tenant.Portal.Areas.Orders.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Feature.PageContent.MenuTiles.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.Components.Footers.ViewModels;

namespace Tenant.Portal.Areas.Orders.ModelBuilders
{
    public class OrderPageModelBuilder : IModelBuilder<OrderPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;

        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;

        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        private readonly IModelBuilder<OrderCatalogViewModel> orderCatalogModelBuilder;

        public OrderPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IModelBuilder<OrderCatalogViewModel> orderCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.orderCatalogModelBuilder = orderCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public OrderPageViewModel BuildModel()
        {
            var viewModel = new OrderPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Catalog = orderCatalogModelBuilder.BuildModel(),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
