using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.Components.LanguageSwitchers.ViewModels;
using Feature.PageContent.Components.Links.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace Account.Shared.Headers.ModelBuilders
{
    public class HeaderModelBuilder : IModelBuilder<HeaderViewModel>
    {
        private readonly IModelBuilder<LogoViewModel> logoModelBuilder;

        private readonly IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel;

        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public HeaderModelBuilder(
            IModelBuilder<LogoViewModel> logoModelBuilder,
            IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.logoModelBuilder = logoModelBuilder;
            this.languageSwitcherViewModel = languageSwitcherViewModel;
            this.globalLocalizer = globalLocalizer;
        }

        public HeaderViewModel BuildModel()
        {
            var links = new List<LinkViewModel>
            {
                new LinkViewModel { Text = this.globalLocalizer["PrivacyPolicy"], Url = "#" },
                new LinkViewModel { Text = this.globalLocalizer["TermsAndConditions"], Url = "#" }
            };

            return new HeaderViewModel
            {
                Logo = this.logoModelBuilder.BuildModel(),
                LanguageSwitcher = this.languageSwitcherViewModel.BuildModel(),
                Links = links
            };
        }
    }
}
