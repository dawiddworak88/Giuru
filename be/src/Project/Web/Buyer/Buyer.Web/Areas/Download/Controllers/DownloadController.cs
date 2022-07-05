using Buyer.Web.Areas.Download.ViewModel;
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

namespace Buyer.Web.Areas.Download.Controllers
{
    public class DownloadController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DownloadPageViewModel> downloadPageModelBuilder;

        public DownloadController(
            IAsyncComponentModelBuilder<ComponentModelBase, DownloadPageViewModel> downloadPageModelBuilder)
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
