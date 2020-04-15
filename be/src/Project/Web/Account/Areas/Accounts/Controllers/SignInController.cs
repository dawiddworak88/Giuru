using Account.Areas.Accounts.ComponentModels;
using Account.Areas.Accounts.Models;
using Account.Areas.Accounts.Validators;
using Account.Areas.Accounts.ViewModels;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Account.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    [AllowAnonymous]
    public class SignInController : BaseController
    {
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IClientStore clientStore;
        private readonly IAuthenticationSchemeProvider schemeProvider;
        private readonly IEventService eventsService;
        private readonly IComponentModelBuilder<SignInComponentModel, SignInViewModel> signInModelBuilder;

        public SignInController(
            IComponentModelBuilder<SignInComponentModel, SignInViewModel> signInModelBuilder,
            IIdentityServerInteractionService interactionService,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService eventsService
            )
        {
            this.signInModelBuilder = signInModelBuilder;
            this.interactionService = interactionService;
            this.clientStore = clientStore;
            this.schemeProvider = schemeProvider;
            this.eventsService = eventsService;
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
                var context = await this.interactionService.GetAuthorizationContextAsync(model.ReturnUrl);

                if (context != null)
                {
                    var subjectId = "1";

                    var username = "dawiddworak88@gmail.com";

                    await this.eventsService.RaiseAsync(new UserLoginSuccessEvent(username, subjectId, username, clientId: context?.ClientId));

                    await HttpContext.SignInAsync(subjectId, username, new AuthenticationProperties {   });

                    return this.Redirect(model.ReturnUrl);
                }
            }

            var viewModel = this.signInModelBuilder.BuildModel(new SignInComponentModel { ReturnUrl = model.ReturnUrl });

            return this.View(viewModel);
        }
    }
}
