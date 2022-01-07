using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Foundation.PageContent.Components.Links.ViewModels;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Foundation.PageContent.Components.LanguageSwitchers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Buyer.Web.Shared.ViewModels.Headers;
using System.Globalization;
using Microsoft.AspNetCore.Routing;
using Foundation.Presentation.Definitions;
using Foundation.PageContent.Components.DrawerMenu.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Buyer.Web.Shared.ModelBuilders.Headers
{
    public class HeaderModelBuilder : IModelBuilder<BuyerHeaderViewModel>
    {
        private readonly IModelBuilder<LogoViewModel> logoModelBuilder;
        private readonly IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;

        public HeaderModelBuilder(
            IModelBuilder<LogoViewModel> logoModelBuilder,
            IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator)
        {
            this.logoModelBuilder = logoModelBuilder;
            this.languageSwitcherViewModel = languageSwitcherViewModel;
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
            this.httpContextAccessor = httpContextAccessor;
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
                IsLoggedIn = this.httpContextAccessor.HttpContext.User.Identity.IsAuthenticated,
                SearchUrl = this.linkGenerator.GetPathByAction("Index", "SearchProducts", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                SearchLabel = this.globalLocalizer.GetString("Search"),
                SearchPlaceholderLabel = this.globalLocalizer.GetString("Search"),
                Logo = this.logoModelBuilder.BuildModel(),
                LanguageSwitcher = this.languageSwitcherViewModel.BuildModel(),
                GetSuggestionsUrl = this.linkGenerator.GetPathByAction("Get", "SearchSuggestionsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                SearchTerm = string.Empty,
                SignInLink = new LinkViewModel
                {
                    Url = this.linkGenerator.GetPathByAction("SignIn", "Account", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name }),
                    Text = this.globalLocalizer.GetString("SignIn")
                },
                SignOutLink = new LinkViewModel
                {
                    Url = this.linkGenerator.GetPathByAction("SignOutNow", "Account", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name }),
                    Text = this.globalLocalizer.GetString("SignOut")
                },
                Links = links
            };
        }
    }
}
