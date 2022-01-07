using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.Presentation.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace Buyer.Web.Shared.MenuTiles
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
                }
            };
        }
    }
}
