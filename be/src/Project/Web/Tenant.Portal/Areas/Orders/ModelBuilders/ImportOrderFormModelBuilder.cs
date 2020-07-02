using Tenant.Portal.Areas.Orders.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Feature.PageContent.MenuTiles.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.Components.Footers.ViewModels;
using Microsoft.Extensions.Localization;
using Feature.Order;
using Foundation.Localization;

namespace Tenant.Portal.Areas.Orders.ModelBuilders
{
    public class ImportOrderFormModelBuilder : IModelBuilder<ImportOrderFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;

        public ImportOrderFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
        }

        public ImportOrderFormViewModel BuildModel()
        {
            var viewModel = new ImportOrderFormViewModel
            {
                DropFilesLabel = this.orderLocalizer["DropFile"],
                DropOrSelectFilesLabel = this.orderLocalizer["DropOrSelectFile"],
                SelectClientLabel = this.orderLocalizer["SelectClient"],
                SaveText = this.orderLocalizer["ImportOrder"],
                GeneralErrorMessage = this.globalLocalizer["AnErrorOccurred"]
            };

            return viewModel;
        }
    }
}
