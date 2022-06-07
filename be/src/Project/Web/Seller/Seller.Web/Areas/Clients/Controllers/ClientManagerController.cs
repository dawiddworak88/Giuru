using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
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
    public class ClientManagerController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientManagerPageViewModel> managerPageModelBuilder;

        public ClientManagerController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientManagerPageViewModel> managerPageModelBuilder)
        {
            this.managerPageModelBuilder = managerPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.managerPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}