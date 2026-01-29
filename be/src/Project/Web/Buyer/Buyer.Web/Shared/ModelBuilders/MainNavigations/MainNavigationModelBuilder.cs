using Foundation.Extensions.ModelBuilders;
using System;
using System.Collections.Generic;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using Foundation.Extensions.ExtensionMethods;
using Buyer.Web.Shared.Repositories.GraphQl;

namespace Buyer.Web.Shared.ModelBuilders.MainNavigations
{
    public class MainNavigationModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly IOptionsMonitor<AppSettings> _settings;
        private readonly IGraphQlRepository _graphQlRepository;

        public MainNavigationModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            IOptionsMonitor<AppSettings> settings,
            LinkGenerator linkGenerator,
            IGraphQlRepository graphQlRepository)
        {
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
            _settings = settings;
            _linkGenerator = linkGenerator;
            _graphQlRepository = graphQlRepository;
        }

        public async Task<MainNavigationViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
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

            if (!string.IsNullOrWhiteSpace(_settings.CurrentValue.DeliveryNoticesUrl))
            {
                links.Insert(1, new LinkViewModel
                {
                    Text = _globalLocalizer.GetString("DeliveryNotices"),
                    Url = _settings.CurrentValue.DeliveryNoticesUrl,
                    Target = "_blank"
                });
            }

            var mainNavigationLinks = await _graphQlRepository.GetMainNavigationLinksAsync(componentModel.Language, _settings.CurrentValue.DefaultCulture);

            foreach (var link in mainNavigationLinks.OrEmptyIfNull())
            {
                links.Add(new LinkViewModel
                {
                    Url = link.Href,
                    Text = link.Label,
                    Target = link.Target
                });
            }

            return new MainNavigationViewModel
            {
                Links = links
            };
        }
    }
}
