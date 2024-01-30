using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ViewModels;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientNotificationTypeController : Controller
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientNotificationTypePageViewModel> _clientNotificationTypePageModelBuilder;

        public ClientNotificationTypeController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientNotificationTypePageViewModel> clientNotificationTypePageModelBuilder)
        {
            _clientNotificationTypePageModelBuilder = clientNotificationTypePageModelBuilder;
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

            var viewModel = await _clientNotificationTypePageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
