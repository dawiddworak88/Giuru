using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.Components.DrawerMenu.ViewModels;
using Foundation.Presentation.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace Buyer.Web.Shared.ModelBuilders.DrawerMenu
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
            };
        }
    }
}
