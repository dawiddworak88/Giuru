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

namespace Seller.Web.Shared.Headers.ModelBuilders
{
    public class HeaderModelBuilder : IModelBuilder<HeaderViewModel>
    {
        private readonly IModelBuilder<LogoViewModel> logoModelBuilder;
        private readonly IModelBuilder<IEnumerable<DrawerMenuViewModel>> drawerMenuModelBuilder;
        private readonly IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel;
        private readonly LinkGenerator linkGenerator;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public HeaderModelBuilder(
            IModelBuilder<LogoViewModel> logoModelBuilder,
            IModelBuilder<IEnumerable<DrawerMenuViewModel>> drawerMenuModelBuilder,
            IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel,
            LinkGenerator linkGenerator,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.logoModelBuilder = logoModelBuilder;
            this.drawerMenuModelBuilder = drawerMenuModelBuilder;
            this.languageSwitcherViewModel = languageSwitcherViewModel;
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
        }

        public HeaderViewModel BuildModel()
        {
            return new HeaderViewModel
            {
                Logo = this.logoModelBuilder.BuildModel(),
                DrawerMenuCategories = this.drawerMenuModelBuilder.BuildModel(),
                LanguageSwitcher = this.languageSwitcherViewModel.BuildModel(),
                LoginLink = new LinkViewModel
                {
                    Url = linkGenerator.GetPathByAction("Index2", "Home", new { Area = "Home", culture = CultureInfo.CurrentUICulture.Name }),
                    Text = this.globalLocalizer["SignIn"]
                },
                Links = new List<LinkViewModel>()
            };
        }
    }
}
