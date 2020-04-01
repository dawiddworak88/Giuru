using Feature.Order;
using Feature.PageContent.MenuTiles.ViewModels;
using Feature.Product.Resources;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Presentation.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace Tenant.Portal.Shared.MenuTiles.ModelBuilders
{
    public class MenuTilesModelBuilder : IModelBuilder<MenuTilesViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        private readonly IStringLocalizer<OrderResources> orderLocalizer;

        private readonly IStringLocalizer<ProductResources> productLocalizer;

        private readonly LinkGenerator linkGenerator;

        public MenuTilesModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public MenuTilesViewModel BuildModel()
        {
            return new MenuTilesViewModel
            {
                Tiles = new List<MenuTileViewModel>
                {
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.ShoppingCart,
                        Title = this.globalLocalizer["Orders"],
                        Url = this.linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Package,
                        Title = this.globalLocalizer["Products"],
                        Url = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Users,
                        Title = this.globalLocalizer["Clients"],
                        Url = this.linkGenerator.GetPathByAction("Index", "Client", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.ProductCards,
                        Title = this.productLocalizer["ProductCards"],
                        Url = "#",
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.OrderCards,
                        Title = this.orderLocalizer["OrderCards"],
                        Url = "#",
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Settings,
                        Title = this.globalLocalizer["Settings"],
                        Url = "#"
                    }
                }
            };
        }
    }
}