using Buyer.Web.Areas.Products.ViewModels.Categories;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class CategoryController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CategoryPageViewModel> categoryPageModelBuilder;

        public CategoryController(IAsyncComponentModelBuilder<ComponentModelBase, CategoryPageViewModel> categoryPageModelBuilder)
        {
            this.categoryPageModelBuilder = categoryPageModelBuilder;
        }

        public async Task<IActionResult> Index(Guid? categoryId)
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name
            };

            var viewModel = await this.categoryPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
