using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Identity.Api.Areas.Accounts.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    [AllowAnonymous]
    public class RegisterController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, RegisterPageViewModel> registerPageModelBuilder;

        public RegisterController(
            IAsyncComponentModelBuilder<ComponentModelBase, RegisterPageViewModel> registerPageModelBuilder)
        {
            this.registerPageModelBuilder = registerPageModelBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
            };

            var viewModel = await this.registerPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
