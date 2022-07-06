using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Download.ViewModel;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Download.ModelBuilders
{
    public class DownloadFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<DownloadResources> downloadLocalizer;
        private readonly LinkGenerator linkGenerator;

        public DownloadFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<DownloadResources> downloadLocalizer,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.downloadLocalizer = downloadLocalizer;
        }

        public async Task<DownloadFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadFormViewModel
            {

            };

            return viewModel;
        }
    }
}
