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
        private readonly IStringLocalizer globalLocalizer;
        private readonly LinkGenerator linkGenerator;

        public SettingsFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<SettingsFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new SettingsFormViewModel
            {
                Title = this.globalLocalizer.GetString("Settings"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                ProductsIndexTriggerUrl = this.linkGenerator.GetPathByAction("Index", "ProductsIndexTriggerApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                ReindexProductsText = this.globalLocalizer.GetString("ReindexProducts")
            };
            
            return viewModel;
        }
    }
}
