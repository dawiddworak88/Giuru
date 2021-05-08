using Foundation.Extensions.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Seller.Web.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class AccountController : BaseController
    {
        public IActionResult SignOut()
        {
            this.Response.Cookies.Delete(".AspNetCore.Identity.Application");
            this.Response.Cookies.Delete("idserv.external");
            this.Response.Cookies.Delete("idserv.session");

            return this.SignOut("Cookies", "oidc");
        }
    }
}
