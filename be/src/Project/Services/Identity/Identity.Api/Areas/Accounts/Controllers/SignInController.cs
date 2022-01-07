using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.Areas.Accounts.Models;
using Identity.Api.Areas.Accounts.Validators;
using Identity.Api.Areas.Accounts.ViewModels;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Identity.Api.Areas.Accounts.Services.UserServices;
using IdentityServer4.Services;

namespace Identity.Api.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    [AllowAnonymous]
    public class SignInController : BaseController
    {
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IUserService userService;
        private readonly IComponentModelBuilder<SignInComponentModel, SignInViewModel> signInModelBuilder;

        public SignInController(
            IIdentityServerInteractionService interactionService,
            IUserService userService,
            IComponentModelBuilder<SignInComponentModel, SignInViewModel> signInModelBuilder
            )
        {
            this.interactionService = interactionService;
            this.userService = userService;
            this.signInModelBuilder = signInModelBuilder;
        }

        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            var viewModel = this.signInModelBuilder.BuildModel(new SignInComponentModel { ReturnUrl = returnUrl });

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SignInModel model)
        {
            var validator = new SignInModelValidator();

            var result = await validator.ValidateAsync(model);

            if (result.IsValid)
            {
                var context = await interactionService.GetAuthorizationContextAsync(model.ReturnUrl);

                if (context != null)
                {
                    if (await this.userService.SignInAsync(model.Email, model.Password, model.ReturnUrl))
                    {
                        return this.Redirect(model.ReturnUrl);
                    }
                }                
            }

            var viewModel = this.signInModelBuilder.BuildModel(new SignInComponentModel { ReturnUrl = model.ReturnUrl });

            return this.View(viewModel);
        }
    }
}
