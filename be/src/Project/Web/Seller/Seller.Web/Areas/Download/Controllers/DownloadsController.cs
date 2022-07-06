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
    public class DownloadsController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DownloadsPageViewModel> downloadsPageModelBuilder;


        public DownloadsController(
            IAsyncComponentModelBuilder<ComponentModelBase, DownloadsPageViewModel> downloadsPageModelBuilder)
        {
            this.downloadsPageModelBuilder = downloadsPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.downloadsPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
