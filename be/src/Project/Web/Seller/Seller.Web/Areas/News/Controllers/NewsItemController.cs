﻿using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.News.ViewModel;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.News.Controllers
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

            var viewModel = await this.newsItemPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}