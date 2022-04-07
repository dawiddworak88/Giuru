using Foundation.Localization;
using Identity.Api.Areas.Home.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Identity.Api.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<ContentController> logger;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public HomeController(
            ILogger<ContentController> logger,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.logger = logger;
            this.globalLocalizer = globalLocalizer;
        }

        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            this.logger.LogError($"An error has occurred in Identity.Api: {requestId}");

            return this.Ok($"{this.globalLocalizer.GetString("ErrorOccurred")} {requestId}");
        }
    }
}
