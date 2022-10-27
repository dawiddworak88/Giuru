using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.News.ViewModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.News.Controllers
{
    [Area("News")]
    public class CategoriesController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CategoriesPageViewModel> categoriesPageModelBuilder;

        public CategoriesController(
            IAsyncComponentModelBuilder<ComponentModelBase, CategoriesPageViewModel> categoriesPageModelBuilder)
        {
            this.categoriesPageModelBuilder = categoriesPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.categoriesPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
