using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Download.ViewModel;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Download.Controllers
{
    [Area("Download")]
    public class DownloadController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DownloadPageViewModel> downloadPageModelBuilder;

        public DownloadController(
            IAsyncComponentModelBuilder<ComponentModelBase, DownloadPageViewModel> downloadPageModelBuilder)
        {
            this.downloadPageModelBuilder = downloadPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.downloadPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
