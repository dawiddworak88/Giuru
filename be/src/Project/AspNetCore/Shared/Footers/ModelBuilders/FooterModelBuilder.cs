using AspNetCore.Shared.Footers.ViewModels;
using Feature.Localization;
using Feature.PageContent.Shared.Links;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Routing;
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
                new LinkViewModel { UniqueId = Guid.NewGuid(), Text = this.globalLocalizer["PriceList"], Url = "#price-list" },
                new LinkViewModel { UniqueId = Guid.NewGuid(), Text = this.globalLocalizer["Contact"], Url = "#contact" }
                
            };

            var viewModel = new FooterViewModel 
            {
                Copyright = this.globalLocalizer["Copyright"]?.Value.Replace(Definitions.Constants.YearToken, DateTime.Now.Year.ToString()),
                Links = links
            };

            return viewModel;
        }
    }
}
