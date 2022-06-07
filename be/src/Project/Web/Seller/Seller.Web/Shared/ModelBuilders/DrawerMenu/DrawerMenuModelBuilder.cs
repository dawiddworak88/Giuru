using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.Components.DrawerMenu.ViewModels;
using Foundation.Presentation.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace Seller.Web.Shared.ModelBuilders.DrawerMenu
{
    public class DrawerMenuModelBuilder : IModelBuilder<IEnumerable<DrawerMenuViewModel>>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;

        public DrawerMenuModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public IEnumerable<DrawerMenuViewModel> BuildModel()
        {
            return new List<DrawerMenuViewModel>
            {
                new DrawerMenuViewModel
                {
                    Items = new List<DrawerMenuItemViewModel>
                    {
                        new DrawerMenuItemViewModel
                        {
                            Icon = IconsConstants.ShoppingCart,
                            Title = this.globalLocalizer.GetString("Orders"),
                            Url = this.linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                        }
                    }
                },
                new DrawerMenuViewModel
                {
                    Items = new List<DrawerMenuItemViewModel>
                    {
                        new DrawerMenuItemViewModel
                        {
                            Icon = IconsConstants.Package,
                            Title = this.globalLocalizer.GetString("Products"),
                            Url = this.linkGenerator.GetPathByAction("Index", "Products", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                        },
                        new DrawerMenuItemViewModel
                        {
                            Icon = IconsConstants.Hexagon,
                            Title = this.globalLocalizer.GetString("ProductAttributes"),
                            Url = this.linkGenerator.GetPathByAction("Index", "ProductAttributes", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                        },
                        new DrawerMenuItemViewModel
                        {
                            Icon = IconsConstants.Grid,
                            Title = this.globalLocalizer.GetString("Categories"),
                            Url = this.linkGenerator.GetPathByAction("Index", "Categories", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
                        }
                    }
                },
                new DrawerMenuViewModel
                {
                    Items = new List<DrawerMenuItemViewModel>
                    {
                         new DrawerMenuItemViewModel
                         {
                            Icon = IconsConstants.News,
                            Title = this.globalLocalizer.GetString("News"),
                            Url = this.linkGenerator.GetPathByAction("Index", "News", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name })
                         },
                         new DrawerMenuItemViewModel
                         {
                            Icon = IconsConstants.Grid,
                            Title = this.globalLocalizer.GetString("NewsCategories"),
                            Url = this.linkGenerator.GetPathByAction("Index", "Categories", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name })
                         }
                    }
                },
                new DrawerMenuViewModel
                {
                    Items = new List<DrawerMenuItemViewModel>
                    {
                         new DrawerMenuItemViewModel
                        {
                            Icon = IconsConstants.Inventory,
                            Title = this.globalLocalizer.GetString("Inventory"),
                            Url = this.linkGenerator.GetPathByAction("Index", "Inventories", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name }),
                        },
                         new DrawerMenuItemViewModel
                        {
                            Icon = IconsConstants.Outlet,
                            Title = this.globalLocalizer.GetString("Outlet"),
                            Url = this.linkGenerator.GetPathByAction("Index", "Outlets", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name }),
                        },
                        new DrawerMenuItemViewModel
                        {
                            Icon = IconsConstants.Warehouses,
                            Title = this.globalLocalizer.GetString("Warehouses"),
                            Url = this.linkGenerator.GetPathByAction("Index", "Warehouses", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name }),
                        }
                    }
                },
                new DrawerMenuViewModel
                {
                    Items = new List<DrawerMenuItemViewModel>
                    {
                        new DrawerMenuItemViewModel
                        {
                            Icon = IconsConstants.Users,
                            Title = this.globalLocalizer.GetString("Clients"),
                            Url = this.linkGenerator.GetPathByAction("Index", "Clients", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                        },
                        new DrawerMenuItemViewModel
                        {
                            Icon = IconsConstants.Briefcase,
                            Title = this.globalLocalizer.GetString("ClientManagers"),
                            Url = this.linkGenerator.GetPathByAction("Index", "ClientManagers", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                        },
                        new DrawerMenuItemViewModel
                        {
                            Icon = IconsConstants.Users,
                            Title = this.globalLocalizer.GetString("ClientsGroups"),
                            Url = this.linkGenerator.GetPathByAction("Index", "ClientGroups", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
                        }
                    }
                },
                new DrawerMenuViewModel
                {
                    Items = new List<DrawerMenuItemViewModel>
                    {
                        new DrawerMenuItemViewModel
                        {
                            Icon = IconsConstants.Settings,
                            Title = this.globalLocalizer.GetString("Settings"),
                            Url = this.linkGenerator.GetPathByAction("Index", "Settings", new { Area = "Settings", culture = CultureInfo.CurrentUICulture.Name })
                        }
                    }
                }
            };
        }
    }
}
