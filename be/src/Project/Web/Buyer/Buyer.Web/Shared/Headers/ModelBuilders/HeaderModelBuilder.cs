using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Foundation.PageContent.Components.Links.ViewModels;
using Microsoft.Extensions.Localization;
using Buyer.Web.Shared.Configurations;
using Microsoft.Extensions.Options;
using Foundation.Localization;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.LanguageSwitchers.ViewModels;

namespace Buyer.Web.Shared.Headers.ModelBuilders
{
    public class HeaderModelBuilder : IModelBuilder<HeaderViewModel>
    {
        private readonly IModelBuilder<LogoViewModel> logoModelBuilder;

        private readonly IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel;

        private readonly IOptions<ServicesEndpointsConfiguration> servicesEndpointsConfiguration;

        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public HeaderModelBuilder(
            IModelBuilder<LogoViewModel> logoModelBuilder,
            IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel,
            IOptions<ServicesEndpointsConfiguration> servicesEndpointsConfiguration,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.logoModelBuilder = logoModelBuilder;
            this.languageSwitcherViewModel = languageSwitcherViewModel;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration;
            this.globalLocalizer = globalLocalizer;
        }

        public HeaderViewModel BuildModel()
        {
            var links = new List<LinkViewModel>
            {
                new LinkViewModel { Text = this.globalLocalizer["PriceList"], Url = "#price-list" },
                new LinkViewModel { Text = this.globalLocalizer["Contact"], Url = "#contact" }
            };

            return new HeaderViewModel
            {
                Logo = this.logoModelBuilder.BuildModel(),
                LanguageSwitcher = this.languageSwitcherViewModel.BuildModel(),
                LoginLink = new LinkViewModel
                {
                    Url = this.servicesEndpointsConfiguration.Value.PortalEndpoint,
                    Text = this.globalLocalizer["Portal"]
                },
                Links = links
            };
        }
    }
}
