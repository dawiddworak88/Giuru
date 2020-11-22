using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Products.ViewModels;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class CategoryController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CategoryPageViewModel> categoryPageModelBuilder;

        public CategoryController(IAsyncComponentModelBuilder<ComponentModelBase, CategoryPageViewModel> categoryPageModelBuilder)
        {
            this.categoryPageModelBuilder = categoryPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.categoryPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
