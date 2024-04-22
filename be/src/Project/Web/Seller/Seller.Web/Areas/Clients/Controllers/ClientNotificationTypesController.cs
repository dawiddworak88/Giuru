using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientNotificationTypesController : Controller
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientNotificationTypesPageViewModel> _notificationTypesModelBuilder;

        public ClientNotificationTypesController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientNotificationTypesPageViewModel> notificationTypesModelBuilder)
        {
            _notificationTypesModelBuilder = notificationTypesModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentCulture.Name,
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await _notificationTypesModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
