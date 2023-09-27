using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Localization.Definitions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using Foundation.Security.Definitions;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.ModelBuilders.Footers
{
    public class FooterModelBuilder : IModelBuilder<FooterViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IOptions<AppSettings> options;

        public FooterModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IOptions<AppSettings> options)
        {
            this.globalLocalizer = globalLocalizer;
            this.options = options;
        }

        public FooterViewModel BuildModel()
        {
            var links = new List<LinkViewModel>
            {
                new LinkViewModel
                {
                    Text = this.globalLocalizer["EltapFabrics"],
                    Target = "_blank",
                    Url = this.options.Value.FabricsWebUrl
                },
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
