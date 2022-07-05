using Buyer.Web.Areas.Download.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Download.ModelBuilders
{
    public class DownloadCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadCatalogViewModel>
    {
        private readonly LinkGenerator linkGenerator;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public DownloadCatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<DownloadCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadCatalogViewModel
            {
                Title = "asd"
            };

            return viewModel;
        }
    }
}
