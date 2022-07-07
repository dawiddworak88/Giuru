using Buyer.Web.Areas.Home.ViewModel.Application;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.Controllers
{
    [Area("Home")]
    [AllowAnonymous]
    public class ApplicationController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ApplicationPageViewModel> applicationPageModelBuilder;

        public ApplicationController(
            IAsyncComponentModelBuilder<ComponentModelBase, ApplicationPageViewModel> applicationPageModelBuilder)
        {
            this.applicationPageModelBuilder = applicationPageModelBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
            };

            var viewModel = await this.applicationPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
