using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ViewModels;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientFieldController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientFieldPageViewModel> _clientFieldPageModelBuilder;

        public ClientFieldController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientFieldPageViewModel> clientFieldPageModelBuilder)
        {
            _clientFieldPageModelBuilder = clientFieldPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await _clientFieldPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
