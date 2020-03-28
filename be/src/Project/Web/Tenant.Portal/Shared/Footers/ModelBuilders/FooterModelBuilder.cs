using Tenant.Portal.Shared.Footers.ViewModels;
using Feature.PageContent.Shared.Footers.Definitions;
using Feature.PageContent.Shared.Links.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using Foundation.Localization;

namespace Tenant.Portal.Shared.Footers.ModelBuilders
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
                Copyright = this.globalLocalizer["Copyright"]?.Value.Replace(Constants.YearToken, DateTime.Now.Year.ToString()),
                Links = new List<LinkViewModel>()
            };

            return viewModel;
        }
    }
}
