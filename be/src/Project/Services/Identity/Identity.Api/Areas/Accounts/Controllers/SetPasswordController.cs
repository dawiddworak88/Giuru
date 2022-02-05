
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.Areas.Accounts.Models;
using Identity.Api.Areas.Accounts.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;

namespace Identity.Api.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    [AllowAnonymous]
    public class SetPasswordController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<SetPasswordComponentModel, SetPasswordViewModel> setPasswordModelBuilder;

        public SetPasswordController(IAsyncComponentModelBuilder<SetPasswordComponentModel, SetPasswordViewModel> signPasswordModelBuilder)
        {
            this.setPasswordModelBuilder = signPasswordModelBuilder;
        }
            
        [HttpGet]
        public async Task<IActionResult> Index(SetPasswordModel model)
        {
            var componentModel = new SetPasswordComponentModel
            {
                Id = model.Id,
                ReturnUrl = string.IsNullOrWhiteSpace(model.ReturnUrl) ? null : HttpUtility.UrlDecode(model.ReturnUrl),
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
            };

            var viewModel = await this.setPasswordModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
