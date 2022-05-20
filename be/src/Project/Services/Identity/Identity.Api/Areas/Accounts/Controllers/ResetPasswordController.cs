using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Identity.Api.Areas.Accounts.ComponentModels;
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
    public class ResetPasswordController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ResetPasswordComponentModel, ResetPasswordViewModel> resetPasswordModelBuilder;

        public ResetPasswordController(
            IAsyncComponentModelBuilder<ResetPasswordComponentModel, ResetPasswordViewModel> resetPasswordModelBuilder)
        {
            this.resetPasswordModelBuilder = resetPasswordModelBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var componentModel = new ResetPasswordComponentModel
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
            };

            var viewModel = await this.resetPasswordModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
