using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Repositories.MainNavigationLinks;
using Foundation.Extensions.ExtensionMethods;
using System.Linq;
using Elastic.CommonSchema;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

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
            IOptionsMonitor<AppSettings> settings,
            LinkGenerator linkGenerator,
            IMainNavigationLinkRepository mainNavigationLinkRepository)
        {
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
            _settings = settings;
            _linkGenerator = linkGenerator;
            _mainNavigationLinkRepository = mainNavigationLinkRepository;
        }

        public async Task<MainNavigationViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var mainNavigationLinks = await _mainNavigationLinkRepository.GetMainNavigationLinksAsync(componentModel.ContentPageKey, componentModel.Language, _settings.CurrentValue.DefaultCulture);

            var links = new List<LinkViewModel>
            {
                new LinkViewModel
                {
                    Text = _orderLocalizer.GetString("MyOrders").Value,
                    Url = _linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
                },
                new LinkViewModel
                {
                    Text = _orderLocalizer.GetString("PlaceOrder").Value,
                    Url = _linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
                },
                new LinkViewModel
                {
                    Text = _globalLocalizer.GetString("AvailableProducts"),
                    Url = _linkGenerator.GetPathByAction("Index", "AvailableProducts", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
                },
                new LinkViewModel
                {
                    Text = _globalLocalizer.GetString("Outlet").Value,
                    Url = _linkGenerator.GetPathByAction("Index", "Outlet", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
                },
                new LinkViewModel
                {
                    Text = _globalLocalizer.GetString("News"),
                    Url = _linkGenerator.GetPathByAction("Index", "News", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name })
                },
                new LinkViewModel
                {
                    Text = _globalLocalizer.GetString("DownloadCenter"),
                    Url = _linkGenerator.GetPathByAction("Index", "DownloadCenter", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name })
                }
            };

            foreach (var link in mainNavigationLinks)
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
