using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.Components.DrawerMenu.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.LanguageSwitchers.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Foundation.Presentation.Definitions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace Buyer.Web.Shared.ModelBuilders.Headers
{
    public class HeaderModelBuilder : IModelBuilder<HeaderViewModel>
    {
        private readonly IModelBuilder<LogoViewModel> logoModelBuilder;
        private readonly IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public HeaderModelBuilder(
            IModelBuilder<LogoViewModel> logoModelBuilder,
            IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel,
            IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.logoModelBuilder = logoModelBuilder;
            this.languageSwitcherViewModel = languageSwitcherViewModel;
            this.httpContextAccessor = httpContextAccessor;
            this.globalLocalizer = globalLocalizer;
        }

        public HeaderViewModel BuildModel()
        {
            return new HeaderViewModel
            {
                IsLoggedIn = this.httpContextAccessor.HttpContext.User.Identity.IsAuthenticated,
                DrawerBackLabel = this.globalLocalizer.GetString("Back"),
                DrawerBackIcon = IconsConstants.ArrowLeft,
                Logo = this.logoModelBuilder.BuildModel(),
                LanguageSwitcher = this.languageSwitcherViewModel.BuildModel(),
                Links = new List<LinkViewModel>()
            };
        }
    }
}
