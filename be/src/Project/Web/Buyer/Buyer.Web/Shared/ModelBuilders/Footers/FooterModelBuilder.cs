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
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IOptions<AppSettings> _options;

        public FooterModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IOptions<AppSettings> options)
        {
            _globalLocalizer = globalLocalizer;
            _options = options;
        }

        public FooterViewModel BuildModel()
        {
            var links = new List<LinkViewModel>
            {
                new LinkViewModel 
                { 
                    Text = _globalLocalizer["TermsConditions"],
                    Url = $"{_options.Value.IdentityUrl}{SecurityConstants.RegulationsEndpoint}"
                },
                new LinkViewModel 
                { 
                    Text = _globalLocalizer["PrivacyPolicy"],
                    Url = $"{_options.Value.IdentityUrl}{SecurityConstants.PrivacyPolicyEndpoint}"
                }
            };

            if (_options.Value.FabricsWebUrl is not null)
            {
                links.Insert(0, new LinkViewModel
                {
                    Text = _globalLocalizer.GetString("EltapFabrics"),
                    Target = "_blank",
                    Url = _options.Value.FabricsWebUrl
                });
            }

            var viewModel = new FooterViewModel 
            {
                Copyright = _globalLocalizer["Copyright"]?.Value.Replace(LocalizationConstants.YearToken, DateTime.Now.Year.ToString()),
                Links = links
            };

            return viewModel;
        }
    }
}
