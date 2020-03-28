using AspNetCore.Shared.Footers.ViewModels;
using Feature.Localization;
using Feature.PageContent.Shared.Footers.Definitions;
using Feature.PageContent.Shared.Links.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;

namespace AspNetCore.Shared.Footers.ModelBuilders
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
                new LinkViewModel { Text = this.globalLocalizer["PriceList"], Url = "#price-list" },
                new LinkViewModel { Text = this.globalLocalizer["Contact"], Url = "#contact" }
                
            };

            var viewModel = new FooterViewModel 
            {
                Copyright = this.globalLocalizer["Copyright"]?.Value.Replace(Constants.YearToken, DateTime.Now.Year.ToString()),
                Links = links
            };

            return viewModel;
        }
    }
}
