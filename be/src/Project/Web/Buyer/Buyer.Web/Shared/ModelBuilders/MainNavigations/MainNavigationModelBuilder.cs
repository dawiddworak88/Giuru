using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Repositories.MainNavigationLinks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Buyer.Web.Shared.ModelBuilders.MainNavigations
{
    public class MainNavigationModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly IOptionsMonitor<AppSettings> _settings;
        private readonly IMainNavigationLinkRepository _mainNavigationLinkRepository;

        public MainNavigationModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator,
            IOptionsMonitor<AppSettings> settings,
            IMainNavigationLinkRepository mainNavigationLinkRepository)
        {
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
            _linkGenerator = linkGenerator;
            _settings = settings;
            _mainNavigationLinkRepository = mainNavigationLinkRepository;
        }

        public async Task<MainNavigationViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var result = await _mainNavigationLinkRepository.GetMainNavigationLinksAsync(componentModel.ContentPageKey, componentModel.Language);

            var links = new List<LinkViewModel>();

            foreach (var link in result)
            {
                links.Add(new LinkViewModel
                {
                    Url = link.Href,
                    Text = link.Label,
                    Target = link.Taget
                });
            }

            return new MainNavigationViewModel
            {
                Links = links
            };
        }
    }
}
