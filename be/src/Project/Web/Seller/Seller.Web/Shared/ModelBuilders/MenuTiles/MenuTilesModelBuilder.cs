using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Presentation.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.Configurations;

namespace Seller.Web.Shared.ModelBuilders.MenuTiles
{
    public class MenuTilesModelBuilder : IModelBuilder<MenuTilesViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IOptionsMonitor<AppSettings> options;

        public MenuTilesModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IOptionsMonitor<AppSettings> options,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
            this.options = options;
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
                        Icon = IconsConstants.Layout,
                        Title = this.globalLocalizer.GetString("ProductCards"),
                        Url = this.linkGenerator.GetPathByAction("Index", "ProductCards", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
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
                        Icon = IconsConstants.News,
                        Title = this.globalLocalizer.GetString("News"),
                        Url = this.linkGenerator.GetPathByAction("Index", "News", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Grid,
                        Title = this.globalLocalizer.GetString("NewsCategories"),
                        Url = this.linkGenerator.GetPathByAction("Index", "Categories", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Users,
                        Title = this.globalLocalizer.GetString("TeamMembers"),
                        Url = this.linkGenerator.GetPathByAction("Index", "TeamMembers", new { Area = "TeamMembers", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Inventory,
                        Title = this.globalLocalizer.GetString("Inventory"),
                        Url = this.linkGenerator.GetPathByAction("Index", "Inventories", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Outlet,
                        Title = this.globalLocalizer.GetString("Outlet"),
                        Url = this.linkGenerator.GetPathByAction("Index", "Outlets", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Warehouses,
                        Title = this.globalLocalizer.GetString("Warehouses"),
                        Url = this.linkGenerator.GetPathByAction("Index", "Warehouses", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Users,
                        Title = this.globalLocalizer.GetString("Clients"),
                        Url = this.linkGenerator.GetPathByAction("Index", "Clients", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Key,
                        Title = this.globalLocalizer.GetString("ClientsRoles"),
                        Url = this.linkGenerator.GetPathByAction("Index", "ClientRoles", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.MapPin,
                        Title = this.globalLocalizer.GetString("Countries"),
                        Url = this.linkGenerator.GetPathByAction("Index", "Countries", new { Area = "Global", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Briefcase,
                        Title = this.globalLocalizer.GetString("ClientManagers"),
                        Url = this.linkGenerator.GetPathByAction("Index", "ClientAccountManagers", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.List,
                        Title = this.globalLocalizer.GetString("ClientsApplications"),
                        Url = this.linkGenerator.GetPathByAction("Index", "ClientApplications", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Grid,
                        Title = this.globalLocalizer.GetString("ClientsGroups"),
                        Url = this.linkGenerator.GetPathByAction("Index", "ClientGroups", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Media,
                        Title = this.globalLocalizer.GetString("Media"),
                        Url = this.linkGenerator.GetPathByAction("Index", "MediaItems", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Content,
                        Title = this.globalLocalizer.GetString("Content"),
                        Target = "_blank",
                        Url = this.options.CurrentValue.ContentUrl
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Download,
                        Title = this.globalLocalizer.GetString("DownloadCenter"),
                        Url = this.linkGenerator.GetPathByAction("Index", "DownloadCenter", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Grid,
                        Title = this.globalLocalizer.GetString("DownloadCenterCategories"),
                        Url = this.linkGenerator.GetPathByAction("Index", "DownloadCenterCategories", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name })
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