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
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<DashboardResources> _dashboardResources;
        private readonly LinkGenerator _linkGenerator;
        private readonly IOptionsMonitor<AppSettings> _options;

        public MenuTilesModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<DashboardResources> dashboardResources,
            IOptionsMonitor<AppSettings> options,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _dashboardResources = dashboardResources;
            _linkGenerator = linkGenerator;
            _options = options;
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
                        Title = _globalLocalizer.GetString("Orders"),
                        Url = _linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.TrendingUp,
                        Title = _dashboardResources.GetString("Dashboard"),
                        Url = _linkGenerator.GetPathByAction("Index", "Dashboard", new { Area = "Dashboard", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Package,
                        Title = _globalLocalizer.GetString("Products"),
                        Url = _linkGenerator.GetPathByAction("Index", "Products", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Layout,
                        Title = _globalLocalizer.GetString("ProductCards"),
                        Url = _linkGenerator.GetPathByAction("Index", "ProductCards", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Hexagon,
                        Title = _globalLocalizer.GetString("ProductAttributes"),
                        Url = _linkGenerator.GetPathByAction("Index", "ProductAttributes", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Grid,
                        Title = _globalLocalizer.GetString("Categories"),
                        Url = _linkGenerator.GetPathByAction("Index", "Categories", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.News,
                        Title = _globalLocalizer.GetString("News"),
                        Url = _linkGenerator.GetPathByAction("Index", "News", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Grid,
                        Title = _globalLocalizer.GetString("NewsCategories"),
                        Url = _linkGenerator.GetPathByAction("Index", "Categories", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Users,
                        Title = _globalLocalizer.GetString("TeamMembers"),
                        Url = _linkGenerator.GetPathByAction("Index", "TeamMembers", new { Area = "TeamMembers", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Inventory,
                        Title = _globalLocalizer.GetString("Inventory"),
                        Url = _linkGenerator.GetPathByAction("Index", "Inventories", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Outlet,
                        Title = _globalLocalizer.GetString("Outlet"),
                        Url = _linkGenerator.GetPathByAction("Index", "Outlets", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Warehouses,
                        Title = _globalLocalizer.GetString("Warehouses"),
                        Url = _linkGenerator.GetPathByAction("Index", "Warehouses", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Users,
                        Title = _globalLocalizer.GetString("Clients"),
                        Url = _linkGenerator.GetPathByAction("Index", "Clients", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Archive,
                        Title = _globalLocalizer.GetString("ClientDeliveryAddresses"),
                        Url = _linkGenerator.GetPathByAction("Index", "ClientDeliveryAddresses", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Key,
                        Title = _globalLocalizer.GetString("ClientsRoles"),
                        Url = _linkGenerator.GetPathByAction("Index", "ClientRoles", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.MapPin,
                        Title = _globalLocalizer.GetString("Countries"),
                        Url = _linkGenerator.GetPathByAction("Index", "Countries", new { Area = "Global", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Briefcase,
                        Title = _globalLocalizer.GetString("ClientManagers"),
                        Url = _linkGenerator.GetPathByAction("Index", "ClientAccountManagers", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.List,
                        Title = _globalLocalizer.GetString("ClientsApplications"),
                        Url = _linkGenerator.GetPathByAction("Index", "ClientApplications", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Grid,
                        Title = _globalLocalizer.GetString("ClientsGroups"),
                        Url = _linkGenerator.GetPathByAction("Index", "ClientGroups", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Media,
                        Title = _globalLocalizer.GetString("Media"),
                        Url = _linkGenerator.GetPathByAction("Index", "MediaItems", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Content,
                        Title = _globalLocalizer.GetString("Content"),
                        Target = "_blank",
                        Url = _options.CurrentValue.ContentUrl
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Download,
                        Title = _globalLocalizer.GetString("DownloadCenter"),
                        Url = _linkGenerator.GetPathByAction("Index", "DownloadCenter", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Grid,
                        Title = _globalLocalizer.GetString("DownloadCenterCategories"),
                        Url = _linkGenerator.GetPathByAction("Index", "DownloadCenterCategories", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name })
                    },
                    new MenuTileViewModel
                    {
                        Icon = IconsConstants.Settings,
                        Title = _globalLocalizer.GetString("Settings"),
                        Url = _linkGenerator.GetPathByAction("Index", "Settings", new { Area = "Settings", culture = CultureInfo.CurrentUICulture.Name })
                    }
                }
            };
        }
    }
}