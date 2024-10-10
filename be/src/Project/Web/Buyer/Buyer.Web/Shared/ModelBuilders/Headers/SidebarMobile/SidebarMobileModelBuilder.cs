using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Repositories.GraphQl;
using Buyer.Web.Shared.ViewModels.Headers.SidebarMobile;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.LanguageSwitchers.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.Headers.SidebarMobile
{
    public class SidebarMobileModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, SidebarMobileViewModel>
    {
        private readonly IModelBuilder<LogoViewModel> _logoModelBuilder;
        private readonly IModelBuilder<LanguageSwitcherViewModel> _languageSwitcherModelBuilder;
        private readonly LinkGenerator _linkGenerator;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IGraphQlRepository _graphQlRepository;
        private readonly IOptionsMonitor<AppSettings> _settings;

        public SidebarMobileModelBuilder(
            IModelBuilder<LogoViewModel> logoModelBuilder,
            IModelBuilder<LanguageSwitcherViewModel> languageSwitcherModelBuilder,
            LinkGenerator linkGenerator,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            IGraphQlRepository graphQlRepository,
            IOptionsMonitor<AppSettings> settings)
        {
            _logoModelBuilder = logoModelBuilder;
            _languageSwitcherModelBuilder = languageSwitcherModelBuilder;
            _linkGenerator = linkGenerator;
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
            _graphQlRepository = graphQlRepository;
            _settings = settings;
        }

        public async Task<SidebarMobileViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new SidebarMobileViewModel
            {
                Logo = _logoModelBuilder.BuildModel(),
                IsLoggedIn = componentModel.IsAuthenticated,
                SignInLink = new LinkViewModel
                {
                    Url = _linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                    Text = _globalLocalizer.GetString("SignIn")
                },
                SignOutLink = new LinkViewModel
                {
                    Url = _linkGenerator.GetPathByAction("SignOutNow", "Account", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name }),
                    Text = _globalLocalizer.GetString("SignOut")
                },
                LanguageSwitcher = _languageSwitcherModelBuilder.BuildModel(),
            };

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

            viewModel.Links = links;

            return viewModel;
        }
    }
}
