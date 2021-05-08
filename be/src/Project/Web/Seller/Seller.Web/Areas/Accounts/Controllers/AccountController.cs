using Foundation.Extensions.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class AccountController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            return this.SignOut("Cookies", "oidc");
        }
    }
}
