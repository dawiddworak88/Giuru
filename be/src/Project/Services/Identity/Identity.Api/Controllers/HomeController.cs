using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Identity.Api.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.Ok(new { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
