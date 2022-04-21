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
    public class ClientGroupController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientGroupPageViewModel> groupPageModelBuilder;

        public ClientGroupController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientGroupPageViewModel> groupPageModelBuilder)
        {
            this.groupPageModelBuilder = groupPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.groupPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}