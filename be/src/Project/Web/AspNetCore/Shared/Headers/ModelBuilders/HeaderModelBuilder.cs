using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Feature.PageContent.Shared.Links.ViewModels;
using Microsoft.Extensions.Localization;
using AspNetCore.Shared.Configurations;
using Microsoft.Extensions.Options;
using Foundation.Localization;
using Feature.PageContent.Shared.Headers.ViewModels;
using Feature.PageContent.Shared.LanguageSwitchers.ViewModels;

namespace AspNetCore.Shared.Headers.ModelBuilders
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
