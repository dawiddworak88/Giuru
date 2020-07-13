using Seller.Portal.Areas.Orders.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Feature.PageContent.MenuTiles.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.Components.Footers.ViewModels;
using Microsoft.Extensions.Localization;
using Feature.Order;
using Microsoft.AspNetCore.Routing;
using System.Globalization;

namespace Seller.Portal.Areas.Orders.ModelBuilders
{
    public class OrderPageModelBuilder : IModelBuilder<OrderPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly LinkGenerator linkGenerator;

        public OrderPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.orderLocalizer = orderLocalizer;
            this.footerModelBuilder = footerModelBuilder;
            this.linkGenerator = linkGenerator;
        }

        public OrderPageViewModel BuildModel()
        {
            var viewModel = new OrderPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Title = this.orderLocalizer["Orders"],
                NewText = this.orderLocalizer["NewOrder"],
                NewUrl = this.linkGenerator.GetPathByAction("Index", "OrderDetail", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                ImportOrderText = this.orderLocalizer["ImportOrder"],
                ImportOrderUrl = this.linkGenerator.GetPathByAction("Index", "ImportOrder", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
