using Feature.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Presentation.Definitions;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace Tenant.Portal.Shared.MenuTiles.ModelBuilders
{
    public class MenuTilesModelBuilder : IModelBuilder<MenuTilesViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public MenuTilesModelBuilder(IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
        }

        public MenuTilesViewModel BuildModel()
        {
            return new MenuTilesViewModel
            {
                Tiles = new List<MenuTileViewModel>
                {
                    new MenuTileViewModel { Icon = IconsConstants.ShoppingCart, Title = this.globalLocalizer["Orders"], Url = "#" },
                    new MenuTileViewModel { Icon = IconsConstants.Package, Title = this.globalLocalizer["Products"], Url = "#" },
                    new MenuTileViewModel { Icon = IconsConstants.Users, Title = this.globalLocalizer["Clients"], Url = "#" },
                    new MenuTileViewModel { Icon = IconsConstants.Settings, Title = this.globalLocalizer["Settings"], Url = "#" }
                }
            };
        }
    }
}
