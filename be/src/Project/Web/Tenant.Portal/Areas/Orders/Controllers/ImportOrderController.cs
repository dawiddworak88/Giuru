using Foundation.Extensions.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Tenant.Portal.Areas.Orders.Controllers
{
    [Area("Orders")]
    public class ImportOrderController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}