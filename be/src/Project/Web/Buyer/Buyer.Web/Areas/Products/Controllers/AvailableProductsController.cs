using Buyer.Web.Areas.Products.ViewModels.AvailableProducts;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class AvailableProductsController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, AvailableProductsPageViewModel> availableProductsPageModelBuilder;

        public AvailableProductsController(IAsyncComponentModelBuilder<ComponentModelBase, AvailableProductsPageViewModel> availableProductsPageModelBuilder)
        {
            this.availableProductsPageModelBuilder = availableProductsPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated
            };

            var viewModel = await this.availableProductsPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
