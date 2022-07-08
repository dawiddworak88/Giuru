using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.DownloadCenter.ViewModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.Controllers
{
    [Area("DownloadCenter")]
    public class DownloadCenterController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterPageViewModel> downloadCenterPageModelBuilder;


        public DownloadCenterController(
            IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterPageViewModel> downloadCenterPageModelBuilder)
        {
            this.downloadCenterPageModelBuilder = downloadCenterPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.downloadCenterPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
