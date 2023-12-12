using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ModelBuilders;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientFieldsController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientFieldsPageViewModel> _clientFieldsPageModelBuilder;

        public ClientFieldsController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientFieldsPageViewModel> clientFieldsPageModelBuilder)
        {
            _clientFieldsPageModelBuilder = clientFieldsPageModelBuilder;
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

            var viewModel = await _clientFieldsPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
