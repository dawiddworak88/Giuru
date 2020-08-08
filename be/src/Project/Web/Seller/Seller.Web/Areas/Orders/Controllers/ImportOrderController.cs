using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Areas.Orders.ViewModel;
using Seller.Web.Shared.ComponentModels;

namespace Seller.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    public class ImportOrderController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ImportOrderPageViewModel> importOrderPageModelBuilder;

        public ImportOrderController(IAsyncComponentModelBuilder<ComponentModelBase, ImportOrderPageViewModel> importOrderPageModelBuilder)
        {
            this.importOrderPageModelBuilder = importOrderPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.importOrderPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}