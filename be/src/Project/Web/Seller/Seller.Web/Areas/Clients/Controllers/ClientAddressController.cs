using Foundation.Extensions.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Foundation.ApiExtensions.Definitions;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using System.Globalization;
using Foundation.Extensions.ModelBuilders;
using Seller.Web.Areas.Clients.ViewModels;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientAddressController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientAddressPageViewModel> _clientAddressPageModelBuilder;

        public ClientAddressController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientAddressPageViewModel> clientAddressPageModelBuilder)
        {
            _clientAddressPageModelBuilder = clientAddressPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
            };

            var viewModel = await _clientAddressPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
