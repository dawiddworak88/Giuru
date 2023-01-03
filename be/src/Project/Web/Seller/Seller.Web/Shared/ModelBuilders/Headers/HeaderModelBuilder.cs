using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.DrawerMenu.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.LanguageSwitchers.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Foundation.Presentation.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Shared.ModelBuilders.Headers
{
    public class HeaderModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel>
    {
        private readonly IModelBuilder<LogoViewModel> logoModelBuilder;
        private readonly IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel;
        private readonly IModelBuilder<IEnumerable<DrawerMenuViewModel>> drawerMenuModelBuilder;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;

        public HeaderModelBuilder(
            IModelBuilder<LogoViewModel> logoModelBuilder,
            IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel,
            IModelBuilder<IEnumerable<DrawerMenuViewModel>> drawerMenuModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            this.logoModelBuilder = logoModelBuilder;
            this.languageSwitcherViewModel = languageSwitcherViewModel;
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
            this.drawerMenuModelBuilder = drawerMenuModelBuilder;
        }

        public async Task<SellerHeaderViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new SellerHeaderViewModel
            {
                WelcomeText = this.globalLocalizer.GetString("Welcome"),
                Name = componentModel.Name,
                IsLoggedIn = componentModel.IsAuthenticated,
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

            return viewModel;
        }
    }
}
