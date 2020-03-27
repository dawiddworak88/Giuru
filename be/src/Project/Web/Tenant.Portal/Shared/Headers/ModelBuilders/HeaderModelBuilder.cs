using Foundation.Extensions.ModelBuilders;
using Tenant.Portal.Shared.Headers.ViewModels;
using Feature.Localization.ViewModels;
using System.Collections.Generic;
using Feature.PageContent.Shared.Links.ViewModels;
using System;
using Feature.Localization;
using Microsoft.Extensions.Localization;
using Tenant.Portal.Shared.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using System.Globalization;

namespace Tenant.Portal.Shared.Headers.ModelBuilders
{
    public class HeaderModelBuilder : IModelBuilder<HeaderViewModel>
    {
        private readonly IModelBuilder<LogoViewModel> logoModelBuilder;

        private readonly IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel;

        private readonly LinkGenerator linkGenerator;

        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public HeaderModelBuilder(
            IModelBuilder<LogoViewModel> logoModelBuilder,
            IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel,
            LinkGenerator linkGenerator,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.logoModelBuilder = logoModelBuilder;
            this.languageSwitcherViewModel = languageSwitcherViewModel;
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
        }

        public HeaderViewModel BuildModel()
        {
            var links = new List<LinkViewModel>
            {
                new LinkViewModel { UniqueId = Guid.NewGuid(), Text = this.globalLocalizer["PriceList"], Url = "#price-list" },
                new LinkViewModel { UniqueId = Guid.NewGuid(), Text = this.globalLocalizer["Contact"], Url = "#contact" }
            };

            return new HeaderViewModel
            {
                Logo = this.logoModelBuilder.BuildModel(),
                LanguageSwitcher = this.languageSwitcherViewModel.BuildModel(),
                LoginLink = new LinkViewModel
                {
                    Url = linkGenerator.GetPathByAction("Index2", "Home", new { Area = "Home", culture = CultureInfo.CurrentUICulture.Name }),
                    Text = this.globalLocalizer["SignIn"]
                },
                Links = links
            };
        }
    }
}
