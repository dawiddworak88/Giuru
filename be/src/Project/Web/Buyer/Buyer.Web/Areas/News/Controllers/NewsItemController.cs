using Buyer.Web.Areas.News.ViewModel;
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

namespace Buyer.Web.Areas.News.Controllers
{
    [Area("News")]
    public class NewsItemController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NewsItemPageViewModel> newsItemPageModelBuilder;

        public NewsItemController(
            IAsyncComponentModelBuilder<ComponentModelBase, NewsItemPageViewModel> newsItemPageModelBuilder)
        {
            this.newsItemPageModelBuilder = newsItemPageModelBuilder;
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(this.Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(this.Request.Cookies[BasketConstants.BasketCookieName])
            };

            var viewModel = await this.newsItemPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
