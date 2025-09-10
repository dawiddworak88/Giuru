using Buyer.Web.Shared.ViewModels.Toasts;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Buyer.Web.Shared.ModelBuilders.Toasts
{
    public class SuccessAddProductToBasketModelBuilder : IModelBuilder<SuccessAddProductToBasketViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly LinkGenerator _linkGenerator;

        public SuccessAddProductToBasketModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
        }

        public SuccessAddProductToBasketViewModel BuildModel()
        {
            return new SuccessAddProductToBasketViewModel
            {
                Title = _globalLocalizer.GetString("SuccessfullyAddedProduct"),
                BasketUrl = _linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                ShowText = _globalLocalizer.GetString("ShowText")
            };
        }
    }
}
