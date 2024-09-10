using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Shared.Definitions.Basket;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class CategoryController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryPageViewModel> _categoryPageModelBuilder;

        public CategoryController(IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryPageViewModel> categoryPageModelBuilder)
        {
            _categoryPageModelBuilder = categoryPageModelBuilder;
        }

        public async Task<IActionResult> Index(Guid? id, string searchTerm)
        {
            var componentModel = new SearchProductsComponentModel
            {
                Id = id,
                SearchTerm = searchTerm,
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(Request.Cookies[BasketConstants.BasketCookieName]),
                ContentPageKey = "categoryPage",
                UserEmail = User.FindFirstValue(ClaimTypes.Email)
            };

            var viewModel = await _categoryPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
