using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Presentation.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace Seller.Web.Shared.ModelBuilders.MenuTiles
{
    public class MenuTilesModelBuilder : IModelBuilder<MenuTilesViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;

        public MenuTilesModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
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
                        Title = this.globalLocalizer.GetString("Orders"),
                        Url = this.linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Package,
                        Title = this.globalLocalizer.GetString("Products"),
                        Url = this.linkGenerator.GetPathByAction("Index", "Products", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Hexagon,
                        Title = this.globalLocalizer.GetString("ProductAttributes"),
                        Url = this.linkGenerator.GetPathByAction("Index", "ProductAttributes", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Grid,
                        Title = this.globalLocalizer.GetString("Categories"),
                        Url = this.linkGenerator.GetPathByAction("Index", "Categories", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Users,
                        Title = this.globalLocalizer.GetString("Clients"),
                        Url = this.linkGenerator.GetPathByAction("Index", "Clients", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Settings,
                        Title = this.globalLocalizer.GetString("Settings"),
                        Url = this.linkGenerator.GetPathByAction("Index", "Settings", new { Area = "Settings", culture = CultureInfo.CurrentUICulture.Name })
                    }
                }
            };
        }
    }
}