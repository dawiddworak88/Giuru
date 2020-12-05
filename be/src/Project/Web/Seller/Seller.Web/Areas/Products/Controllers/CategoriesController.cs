using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Products.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Categoriess.Controllers
{
    [Area("Products")]
    public class CategoriesController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CategoriesPageViewModel> categoriesPageModelBuilder;

        public CategoriesController(IAsyncComponentModelBuilder<ComponentModelBase, CategoriesPageViewModel> categoriesPageModelBuilder)
        {
            this.categoriesPageModelBuilder = categoriesPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.categoriesPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
