using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Dashboard.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class DashboardController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DashboardPageViewModel> _dashboardModelBuilder;

        public DashboardController(
            IAsyncComponentModelBuilder<ComponentModelBase, DashboardPageViewModel> dashboardModelBuilder)
        {
            _dashboardModelBuilder = dashboardModelBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await _dashboardModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
