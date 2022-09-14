using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using Seller.Web.Shared.ViewModels;
using System.Threading.Tasks;

namespace Seller.Web.Shared.ModelBuilders.Dialogs
{
    public class QRCodeDialogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, QRCodeDialogViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public QRCodeDialogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<QRCodeDialogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new QRCodeDialogViewModel
            {
                Title = this.globalLocalizer.GetString("QRCode"),
                CloseLabel = this.globalLocalizer.GetString("Close"),
                DownloadLabel = this.globalLocalizer.GetString("Download")
            };

            return viewModel;
        }
    }
}
