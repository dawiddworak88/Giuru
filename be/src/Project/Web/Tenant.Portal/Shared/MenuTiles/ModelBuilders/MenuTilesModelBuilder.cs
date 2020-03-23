using Feature.Localization;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using Tenant.Portal.Shared.MenuTiles.ViewModels;

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
                OrdersText = this.globalLocalizer["Orders"],
                ProductsText = this.globalLocalizer["Products"],
                ClientsText = this.globalLocalizer["Clients"],
                SettingsText = this.globalLocalizer["Settings"]
            };
        }
    }
}
