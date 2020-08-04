using Feature.PageContent.Components.Links.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using Foundation.Localization;
using Feature.PageContent.Components.Footers.ViewModels;
using Foundation.Localization.Definitions;

namespace Seller.Web.Shared.Footers.ModelBuilders
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
            var viewModel = new FooterViewModel 
            {
                Copyright = this.globalLocalizer["Copyright"]?.Value.Replace(LocalizationConstants.YearToken, DateTime.Now.Year.ToString()),
                Links = new List<LinkViewModel>()
            };

            return viewModel;
        }
    }
}
