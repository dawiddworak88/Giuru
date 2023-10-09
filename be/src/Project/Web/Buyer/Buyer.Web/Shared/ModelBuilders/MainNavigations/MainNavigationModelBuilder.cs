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

            if (result is not null)
            {
                foreach (var link in result)
                {
                    links.Add(new LinkViewModel
                    {
                        Url = link.Href,
                        Text = link.Label,
                        Target = link.Taget
                    });
                }
            }

            //var links = new List<LinkViewModel>
            //{
            //    new LinkViewModel
            //    {
            //        Text = _orderLocalizer.GetString("MyOrders").Value,
            //        Url = _linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
            //    },
            //    new LinkViewModel
            //    {
            //        Text = _orderLocalizer.GetString("PlaceOrder").Value,
            //        Url = _linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
            //    },
            //    new LinkViewModel
            //    {
            //        Text = _globalLocalizer.GetString("AvailableProducts"),
            //        Url = _linkGenerator.GetPathByAction("Index", "AvailableProducts", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
            //    },
            //    new LinkViewModel
            //    {
            //        Text = _globalLocalizer.GetString("Outlet").Value,
            //        Url = _linkGenerator.GetPathByAction("Index", "Outlet", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
            //    },
            //    new LinkViewModel
            //    {
            //        Text = _globalLocalizer.GetString("News"),
            //        Url = _linkGenerator.GetPathByAction("Index", "News", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name })
            //    },
            //    new LinkViewModel
            //    {
            //        Text = _globalLocalizer.GetString("DownloadCenter"),
            //        Url = _linkGenerator.GetPathByAction("Index", "DownloadCenter", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name })
            //    },
            //    new LinkViewModel
            //    {
            //        Text = _globalLocalizer.GetString("MakeComplaint"),
            //        Target = "_blank",
            //        Url = _settings.CurrentValue.MakeComplaintUrl
            //    }
            //};

            //if (_settings.CurrentValue.FabricsWebUrl is not null)
            //{
            //    links.Insert(4, new LinkViewModel
            //    {
            //        Text = _globalLocalizer.GetString("EltapFabrics"),
            //        Target = "_blank",
            //        Url = _settings.CurrentValue.FabricsWebUrl
            //    });
            //}

            return new MainNavigationViewModel
            {
                Links = links
            };
        }
    }
}
