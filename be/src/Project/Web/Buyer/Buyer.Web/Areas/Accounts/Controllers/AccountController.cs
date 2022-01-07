using Foundation.Extensions.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Buyer.Web.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class AccountController : BaseController
    {
        public IActionResult SignIn()
        {
            return default;
        }

        public IActionResult SignOutNow()
        {
            return this.SignOut("Cookies", "oidc");
        }
    }
}
