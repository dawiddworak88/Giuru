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
    public class ClientApprovalsController : Controller
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientApprovalsPageViewModel> _clientApprovalsPageModelBuilder;

        public ClientApprovalsController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientApprovalsPageViewModel> clientApprovalsPageModelBuilder)
        {
            _clientApprovalsPageModelBuilder = clientApprovalsPageModelBuilder;
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

            var viewModel = await _clientApprovalsPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
