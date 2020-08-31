using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Foundation.Localization;

namespace Buyer.Web.Shared.Headers.ModelBuilders
{
    public class MainNavigationModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public MainNavigationModelBuilder(IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<MainNavigationViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var links = new List<LinkViewModel>
            { 
                new LinkViewModel 
                {
                    Text = this.globalLocalizer["Categories"],
                    Url = "#"
                },
                new LinkViewModel
                {
                    Text = this.globalLocalizer["Sell"],
                    Url = "#"
                }
            };

            return new MainNavigationViewModel
            {
                Links = links
            };
        }
    }
}
