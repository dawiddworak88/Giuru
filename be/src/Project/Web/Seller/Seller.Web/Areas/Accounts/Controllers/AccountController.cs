using Foundation.Extensions.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Seller.Web.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class AccountController : BaseController
    {
        public IActionResult SignOutNow()
        {
            return this.SignOut("Cookies", "oidc");
        }
    }
}
