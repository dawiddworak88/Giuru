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
    public class ClientRoleController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientRolePageViewModel> rolePageModelBuilder;

        public ClientRoleController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientRolePageViewModel> rolePageModelBuilder)
        {
            this.rolePageModelBuilder = rolePageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Name = this.User.Identity.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.rolePageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}