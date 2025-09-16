using Buyer.Web.Shared.ViewModels.Modals;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.Extensions.Localization;

namespace Buyer.Web.Shared.ModelBuilders.Modals
{
    public class PriceModalModelBuilder : IModelBuilder<PriceModalViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public PriceModalModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            _globalLocalizer = globalLocalizer;
        }

        public PriceModalViewModel BuildModel()
        {
            var viewModel = new PriceModalViewModel
            {
                Title = "Cena zawiera",
                Note = "*Więcej info gdzies tam"
            };

            return viewModel;
        }
    }
}
