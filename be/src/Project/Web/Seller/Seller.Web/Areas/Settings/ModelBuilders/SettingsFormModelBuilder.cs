using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Settings.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Settings.ModelBuilders
{
    public class SettingsFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, SettingsFormViewModel>
    {
        private readonly IStringLocalizer _globalLocalizer;
        private readonly LinkGenerator _linkGenerator;

        public SettingsFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
        }

        public async Task<SettingsFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new SettingsFormViewModel
            {
                Title = _globalLocalizer.GetString("Settings"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                ProductsIndexTriggerUrl = _linkGenerator.GetPathByAction("Index", "ProductsIndexTriggerApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                ReindexProductsText = _globalLocalizer.GetString("ReindexProducts")
            };

            return viewModel;
        }
    }
}
