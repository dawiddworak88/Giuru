using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class CategoryController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryPageViewModel> categoryPageModelBuilder;

        public CategoryController(IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryPageViewModel> categoryPageModelBuilder)
        {
            this.categoryPageModelBuilder = categoryPageModelBuilder;
        }

        public async Task<IActionResult> Index(Guid? id, string searchTerm)
        {
            var componentModel = new SearchProductsComponentModel
            {
                Id = id,
                SearchTerm = searchTerm,
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated
            };

            var viewModel = await this.categoryPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
