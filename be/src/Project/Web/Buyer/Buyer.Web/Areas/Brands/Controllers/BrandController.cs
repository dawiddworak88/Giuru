using Foundation.Extensions.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Buyer.Web.Areas.Brands.Controllers
{
    [Area("Brands")]
    public class BrandController : BaseController
    {
        public IActionResult Index(Guid? id)
        {
            return this.View();
        }
    }
}
