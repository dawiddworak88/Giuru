using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Foundation.PageContent.Components.Links.ViewModels;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using Foundation.Localization;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.LanguageSwitchers.ViewModels;
using Foundation.PageContent.Components.DrawerMenu.ViewModels;
using Foundation.Presentation.Definitions;
using Microsoft.AspNetCore.Http;

namespace Seller.Web.Shared.ModelBuilders.Headers
{
    public class HeaderModelBuilder : IModelBuilder<HeaderViewModel>
    {
        private readonly IModelBuilder<LogoViewModel> logoModelBuilder;
        private readonly IModelBuilder<IEnumerable<DrawerMenuViewModel>> drawerMenuModelBuilder;
        private readonly IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel;
        private readonly LinkGenerator linkGenerator;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public HeaderModelBuilder(
            IModelBuilder<LogoViewModel> logoModelBuilder,
            IModelBuilder<IEnumerable<DrawerMenuViewModel>> drawerMenuModelBuilder,
            IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.logoModelBuilder = logoModelBuilder;
            this.drawerMenuModelBuilder = drawerMenuModelBuilder;
            this.languageSwitcherViewModel = languageSwitcherViewModel;
            this.linkGenerator = linkGenerator;
            this.httpContextAccessor = httpContextAccessor;
            this.globalLocalizer = globalLocalizer;
        }

        public HeaderViewModel BuildModel()
        {
            return new HeaderViewModel
            {
                IsLoggedIn = this.httpContextAccessor.HttpContext.User.Identity.IsAuthenticated,
                SignOutLink = new LinkViewModel
                { 
                    Url = this.linkGenerator.GetPathByAction("SignOutNow", "Account", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name }),
                    Text = this.globalLocalizer.GetString("SignOut")
                },
                DrawerBackLabel = this.globalLocalizer.GetString("Back"),
                DrawerBackIcon = IconsConstants.ArrowLeft,
                Logo = this.logoModelBuilder.BuildModel(),
                DrawerMenuCategories = this.drawerMenuModelBuilder.BuildModel(),
                LanguageSwitcher = this.languageSwitcherViewModel.BuildModel(),
                Links = new List<LinkViewModel>()
            };
        }
    }
}
