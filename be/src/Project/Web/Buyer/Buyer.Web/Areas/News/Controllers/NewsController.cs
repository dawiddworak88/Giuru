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
    public class NewsController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NewsPageViewModel> newsPageModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NewsItemPageViewModel> newsItemPageModelBuilder;

        public NewsController(
            IAsyncComponentModelBuilder<ComponentModelBase, NewsPageViewModel> newsPageModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NewsItemPageViewModel> newsItemPageModelBuilder)
        {
            this.newsPageModelBuilder = newsPageModelBuilder;
            this.newsItemPageModelBuilder = newsItemPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                ContentPageKey = "newsPage",
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(this.Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(this.Request.Cookies[BasketConstants.BasketCookieName])
            };

            var viewModel = await this.newsPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Item(Guid? id)
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
