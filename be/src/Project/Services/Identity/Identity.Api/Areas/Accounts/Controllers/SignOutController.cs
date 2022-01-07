using Foundation.Extensions.Controllers;
using Identity.Api.Areas.Accounts.Services.UserServices;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    [AllowAnonymous]
    public class SignOutController : BaseController
    {
        private readonly IUserService userService;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IEventService events;

        public SignOutController(
            IUserService userService,
            IIdentityServerInteractionService interaction,
            IEventService events)
        {
            this.userService = userService;
            this.interaction = interaction;
            this.events = events;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string logoutId)
        {
            if (this.User?.Identity.IsAuthenticated == true)
            {
                await this.userService.SignOutAsync();

                await this.events.RaiseAsync(new UserLogoutSuccessEvent(this.User.GetSubjectId(), this.User.GetDisplayName()));
            }

            this.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            var logout = await this.interaction.GetLogoutContextAsync(logoutId);

            return this.Redirect(logout?.PostLogoutRedirectUri);
        }
    }
}
