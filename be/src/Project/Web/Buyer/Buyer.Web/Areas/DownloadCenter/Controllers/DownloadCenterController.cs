using Buyer.Web.Areas.DownloadCenter.ViewModel;
using Buyer.Web.Shared.Definitions.Basket;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.Controllers
{
    [Area("DownloadCenter")]
    public class DownloadCenterController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterPageViewModel> downloadPageModelBuilder;

        public DownloadCenterController(
            IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterPageViewModel> downloadPageModelBuilder)
        {
            this.downloadPageModelBuilder = downloadPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(this.Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(this.Request.Cookies[BasketConstants.BasketCookieName])
            };

            var viewModel = await this.downloadPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
