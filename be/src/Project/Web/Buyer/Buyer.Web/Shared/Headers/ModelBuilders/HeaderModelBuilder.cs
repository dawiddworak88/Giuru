using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Foundation.PageContent.Components.Links.ViewModels;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Foundation.PageContent.Components.LanguageSwitchers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Buyer.Web.Shared.Headers.ViewModels;
using System.Globalization;
using Microsoft.AspNetCore.Routing;

namespace Buyer.Web.Shared.Headers.ModelBuilders
{
    public class HeaderModelBuilder : IModelBuilder<BuyerHeaderViewModel>
    {
        private readonly IModelBuilder<LogoViewModel> logoModelBuilder;
        private readonly IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;

        public HeaderModelBuilder(
            IModelBuilder<LogoViewModel> logoModelBuilder,
            IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel,
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            this.logoModelBuilder = logoModelBuilder;
            this.languageSwitcherViewModel = languageSwitcherViewModel;
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public BuyerHeaderViewModel BuildModel()
        {
            var links = new List<LinkViewModel>
            {
                new LinkViewModel { Text = this.globalLocalizer["PriceList"], Url = "#price-list" },
                new LinkViewModel { Text = this.globalLocalizer["Contact"], Url = "#contact" }
            };

            return new BuyerHeaderViewModel
            {
                Logo = this.logoModelBuilder.BuildModel(),
                LanguageSwitcher = this.languageSwitcherViewModel.BuildModel(),
                LoginLink = new LinkViewModel
                {
                    Url = "/",
                    Text = this.globalLocalizer.GetString("Portal")
                },
                SearchLabel = this.globalLocalizer.GetString("Search"),
                SearchPlaceholderLabel = this.globalLocalizer.GetString("Search"),
                SearchUrl = this.linkGenerator.GetPathByAction("Index", "SearchProducts", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                GetSuggestionsUrl = this.linkGenerator.GetPathByAction("Get", "SearchSuggestionsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                SearchTerm = string.Empty,
                Links = links
            };
        }
    }
}
