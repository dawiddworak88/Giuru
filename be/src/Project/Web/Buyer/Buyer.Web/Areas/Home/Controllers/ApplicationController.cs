using Foundation.Extensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Buyer.Web.Areas.Home.Controllers
{
    [Area("Home")]
    [AllowAnonymous]
    public class ApplicationController : BaseController
    {
    }
}
