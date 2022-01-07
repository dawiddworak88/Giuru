
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.Areas.Accounts.Models;
using Identity.Api.Areas.Accounts.Services.UserServices;
using Identity.Api.Areas.Accounts.ViewModels;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    [AllowAnonymous]
    public class SetPasswordController : BaseController
    {
        private readonly IUserService userService;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IEventService events;
        private readonly IAsyncComponentModelBuilder<SetPasswordComponentModel, SetPasswordViewModel> signPasswordModelBuilder;

        public SetPasswordController(
            IUserService userService,
            IIdentityServerInteractionService interaction,
            IAsyncComponentModelBuilder<SetPasswordComponentModel, SetPasswordViewModel> signPasswordModelBuilder,
            IEventService events)
        {
            this.userService = userService;
            this.interaction = interaction;
            this.events = events;
            this.signPasswordModelBuilder = signPasswordModelBuilder;
        }
            
        [HttpGet]
        public async Task<IActionResult> Index(SetPasswordModel model)
        {
            var componentModel = new SetPasswordComponentModel
            {
                Id = model.Id,
                ReturnUrl = model.ReturnUrl,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
            };

            var viewModel = await this.signPasswordModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save(SetPasswordModel model)
        {
            return default;
        }
    }
}
