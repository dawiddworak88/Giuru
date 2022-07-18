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
using Identity.Api.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Localization;
using Feature.Account;

namespace Identity.Api.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    [AllowAnonymous]
    public class SignInController : BaseController
    {
        private readonly IOptions<AppSettings> settings;
        private readonly IStringLocalizer<AccountResources> accountLocalizer;
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IUserService userService;
        private readonly IComponentModelBuilder<SignInComponentModel, SignInViewModel> signInModelBuilder;

        public SignInController(
            IOptions<AppSettings> settings,
            IStringLocalizer<AccountResources> accountLocalizer,
            IIdentityServerInteractionService interactionService,
            IUserService userService,
            IComponentModelBuilder<SignInComponentModel, SignInViewModel> signInModelBuilder
            )
        {
            this.interactionService = interactionService;
            this.userService = userService;
            this.signInModelBuilder = signInModelBuilder;
            this.settings = settings;
            this.accountLocalizer = accountLocalizer;
        }

        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            var signInComponentModel = new SignInComponentModel
            {
                ReturnUrl = returnUrl,
                DevelopersEmail = this.settings.Value.DevelopersEmail
            };

            var viewModel = this.signInModelBuilder.BuildModel(signInComponentModel);

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
                    if (await this.userService.SignInAsync(model.Email, model.Password, model.ReturnUrl, context.Client.ClientId))
                    {
                        return this.Redirect(model.ReturnUrl);
                    }
                }                
            }

            var viewModel = this.signInModelBuilder.BuildModel(new SignInComponentModel { ErrorMessage = this.accountLocalizer.GetString("InvalidPassword").Value, ReturnUrl = model.ReturnUrl, DevelopersEmail = this.settings.Value.DevelopersEmail });

            return this.View(viewModel);
        }
    }
}
