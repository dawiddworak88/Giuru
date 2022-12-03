using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.DownloadCenter.ViewModel;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.Controllers
{
    [Area("DownloadCenter")]
    public class DownloadCenterItemController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterItemPageViewModel> downloadCenterItemPageModelBuilder;

        public DownloadCenterItemController(
            IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterItemPageViewModel> downloadCenterItemPageModelBuilder)
        {
            this.downloadCenterItemPageModelBuilder = downloadCenterItemPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.downloadCenterItemPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
