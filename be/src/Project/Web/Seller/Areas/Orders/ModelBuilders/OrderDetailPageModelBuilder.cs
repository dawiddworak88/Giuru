using Seller.Web.Areas.Orders.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Microsoft.Extensions.Localization;
using Feature.Order;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderDetailPageModelBuilder : IModelBuilder<OrderDetailPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;

        public OrderDetailPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.orderLocalizer = orderLocalizer;
            this.footerModelBuilder = footerModelBuilder;
        }

        public OrderDetailPageViewModel BuildModel()
        {
            var viewModel = new OrderDetailPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Title = this.orderLocalizer["Order"],
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
