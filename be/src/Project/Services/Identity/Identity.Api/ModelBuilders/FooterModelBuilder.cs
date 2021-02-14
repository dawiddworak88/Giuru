using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Localization.Definitions;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using System.Globalization;

namespace Identity.Api.ModelBuilders
{
    public class FooterModelBuilder : IModelBuilder<FooterViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;

        public FooterModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public FooterViewModel BuildModel()
        {
            var links = new List<LinkViewModel>
            {
                new LinkViewModel 
                { 
                    Text = this.globalLocalizer["PrivacyPolicy"], 
                    Url = this.linkGenerator.GetPathByAction("PrivacyPolicy", "Content", new { Area = "Home", culture = CultureInfo.CurrentUICulture.Name })
                },
                new LinkViewModel 
                { 
                    Text = this.globalLocalizer["TermsAndConditions"], 
                    Url = this.linkGenerator.GetPathByAction("Regulations", "Content", new { Area = "Home", culture = CultureInfo.CurrentUICulture.Name })
                }
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
