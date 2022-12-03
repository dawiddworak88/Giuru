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
    public class ClientApplicationController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientApplicationPageViewModel> applicationPageModelBuilder;

        public ClientApplicationController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientApplicationPageViewModel> applicationPageModelBuilder)
        {
            this.applicationPageModelBuilder = applicationPageModelBuilder;
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

            var viewModel = await this.applicationPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}