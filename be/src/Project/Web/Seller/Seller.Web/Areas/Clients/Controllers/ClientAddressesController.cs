using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
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
    public class ClientAddressesController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientAddressesPageViewModel> _clientAddressesPageModelBuilder;

        public ClientAddressesController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientAddressesPageViewModel> clientAddressesPageModelBuilder)
        {
            _clientAddressesPageModelBuilder = clientAddressesPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await _clientAddressesPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
