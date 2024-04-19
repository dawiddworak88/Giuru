using Buyer.Web.Shared.Configurations;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.LanguageSwitchers.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Foundation.Presentation.Definitions;
using Foundation.Security.Definitions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Buyer.Web.Shared.ModelBuilders.Headers
{
    public class HeaderModelBuilder : IModelBuilder<HeaderViewModel>
    {
        private readonly IModelBuilder<LogoViewModel> logoModelBuilder;
        private readonly IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IOptions<AppSettings> options;

        public HeaderModelBuilder(
            IModelBuilder<LogoViewModel> logoModelBuilder,
            IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> options,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.logoModelBuilder = logoModelBuilder;
            this.languageSwitcherViewModel = languageSwitcherViewModel;
            this.httpContextAccessor = httpContextAccessor;
            this.globalLocalizer = globalLocalizer;
            this.options = options;
        }

        public HeaderViewModel BuildModel()
        {
            return new HeaderViewModel
            {
                IsLoggedIn = this.httpContextAccessor.HttpContext.User.Identity.IsAuthenticated,
                DrawerBackLabel = this.globalLocalizer.GetString("Back"),
                SearchLabel = this.globalLocalizer.GetString("Search"),
                SearchPlaceholderLabel = this.globalLocalizer.GetString("Search"),
                DrawerBackIcon = IconsConstants.ArrowLeft,
                Logo = this.logoModelBuilder.BuildModel(),
                LanguageSwitcher = this.languageSwitcherViewModel.BuildModel(),
                Links = new List<LinkViewModel>()
                {
                    new LinkViewModel
                    {
                        Text = this.globalLocalizer["TermsConditions"],
                        Url = $"{this.options.Value.IdentityUrl}{SecurityConstants.RegulationsEndpoint}"
                    },
                    new LinkViewModel
                    {
                        Text = this.globalLocalizer["PrivacyPolicy"],
                        Url = $"{this.options.Value.IdentityUrl}{SecurityConstants.PrivacyPolicyEndpoint}"
                    }
                }
            };
        }
    }
}
