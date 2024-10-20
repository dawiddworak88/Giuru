﻿using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ViewModels.SearchProducts;
using Buyer.Web.Shared.Definitions.Basket;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class SearchProductsController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<SearchProductsComponentModel, SearchProductsPageViewModel> searchProductsPageModelBuilder;

        public SearchProductsController(IAsyncComponentModelBuilder<SearchProductsComponentModel, SearchProductsPageViewModel> searchProductsPageModelBuilder)
        {
            this.searchProductsPageModelBuilder = searchProductsPageModelBuilder;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var componentModel = new SearchProductsComponentModel
            {
                ContentPageKey = "searchProductsPage",
                SearchTerm = searchTerm,
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(this.Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(this.Request.Cookies[BasketConstants.BasketCookieName])
            };

            var viewModel = await this.searchProductsPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
