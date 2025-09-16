using Buyer.Web.Shared.ViewModels.Modals;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;

namespace Buyer.Web.Shared.ModelBuilders.Modals
{
    public class PriceModalModelBuilder : IComponentModelBuilder<ComponentModelBase, PriceModalViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public PriceModalModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            _globalLocalizer = globalLocalizer;
        }

        public PriceModalViewModel BuildModel(ComponentModelBase componentModel)
        {
            var viewModel = new PriceModalViewModel
            {
                PriceInclusionTitle = "Cena zawiera",
                PriceNote = "*Więcej info gdzies tam"
            };

            return viewModel;
        }
    }
}
