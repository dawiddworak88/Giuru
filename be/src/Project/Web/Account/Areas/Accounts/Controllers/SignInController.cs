using Account.Areas.Accounts.ViewModels;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IModelBuilder<SignInViewModel> signInModelBuilder;

        public SignInController(
            IModelBuilder<SignInViewModel> signInModelBuilder,
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

        public async Task<IActionResult> Index(string returnUrl)
        {
            var context = await this.interactionService.GetAuthorizationContextAsync(returnUrl);

            var viewModel = this.signInModelBuilder.BuildModel();

            return this.View(viewModel);
        }
    }
}
