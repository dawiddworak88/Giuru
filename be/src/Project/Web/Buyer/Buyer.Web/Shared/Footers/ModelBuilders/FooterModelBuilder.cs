using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Localization.Definitions;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.Footers.ModelBuilders
{
    public class FooterModelBuilder : IModelBuilder<FooterViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public FooterModelBuilder(IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
        }

        public FooterViewModel BuildModel()
        {
            var links = new List<LinkViewModel>
            {
                new LinkViewModel { Text = this.globalLocalizer["TermsConditions"], Url = "#" },
                new LinkViewModel { Text = this.globalLocalizer["PrivacyPolicy"], Url = "#" },
                new LinkViewModel { Text = this.globalLocalizer["Contact"], Url = "#" },
            };

            var viewModel = new FooterViewModel 
            {
                Copyright = this.globalLocalizer["Copyright"]?.Value.Replace(LocalizationConstants.YearToken, DateTime.Now.Year.ToString()),
                Links = links
            };

            return viewModel;
        }
    }
}
