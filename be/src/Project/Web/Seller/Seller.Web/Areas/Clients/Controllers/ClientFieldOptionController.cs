using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ComponentModels;
using Seller.Web.Areas.Clients.ViewModels;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientFieldOptionController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ClientFieldOptionComponentModel, ClientFieldOptionPageViewModel> _clientFieldOptionPageModelBuilder;

        public ClientFieldOptionController(IAsyncComponentModelBuilder<ClientFieldOptionComponentModel, ClientFieldOptionPageViewModel> clientFieldOptionPageModelBuilder)
        {
            _clientFieldOptionPageModelBuilder = clientFieldOptionPageModelBuilder;
        }

        public async Task<IActionResult> New(Guid? id)
        {
            var componentModel = new ClientFieldOptionComponentModel
            {
                ClientFieldOptionId = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
            };

            var viewModel = await _clientFieldOptionPageModelBuilder.BuildModelAsync(componentModel);

            return View("Edit", viewModel);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ClientFieldOptionComponentModel
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
            };

            var viewModel = await _clientFieldOptionPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
