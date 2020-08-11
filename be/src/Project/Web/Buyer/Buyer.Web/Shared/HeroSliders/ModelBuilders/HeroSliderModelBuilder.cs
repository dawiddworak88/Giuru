using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Foundation.PageContent.Components.HeroSliders.ViewModels;

namespace Buyer.Web.Shared.Headers.ModelBuilders
{
    public class HeroSliderModelBuilder : IModelBuilder<HeroSliderViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public HeroSliderModelBuilder(IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
        }

        public HeroSliderViewModel BuildModel()
        {
            var items = new List<HeroSliderItemViewModel>();

            return new HeroSliderViewModel
            {
                Items = items
            };
        }
    }
}
