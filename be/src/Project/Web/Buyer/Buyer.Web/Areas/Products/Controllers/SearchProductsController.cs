using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ViewModels.SearchProducts;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
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
                SearchTerm = searchTerm,
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name
            };

            var viewModel = await this.searchProductsPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
