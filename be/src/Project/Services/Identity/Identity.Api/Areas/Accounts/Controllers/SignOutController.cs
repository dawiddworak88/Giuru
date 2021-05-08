using Foundation.Extensions.Controllers;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
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
        private readonly IIdentityServerInteractionService interaction;
        private readonly IEventService events;

        public SignOutController(
            IIdentityServerInteractionService interaction,
            IEventService events)
        {
            this.interaction = interaction;
            this.events = events;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string logoutId)
        {
            if (this.User?.Identity.IsAuthenticated == true)
            {
                await this.HttpContext.SignOutAsync();

                await this.events.RaiseAsync(new UserLogoutSuccessEvent(this.User.GetSubjectId(), this.User.GetDisplayName()));
            }

            this.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            var logout = await this.interaction.GetLogoutContextAsync(logoutId);

            return this.Redirect(logout?.PostLogoutRedirectUri);
        }
    }
}
