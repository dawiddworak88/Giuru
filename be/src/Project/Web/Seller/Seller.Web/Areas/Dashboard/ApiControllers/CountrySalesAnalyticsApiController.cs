using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Dashboard.RequestModels;
using System.Net;

namespace Seller.Web.Areas.Dashboard.ApiControllers
{
    [Area("Dashboard")]
    public class CountrySalesAnalyticsApiController : BaseApiController
    {
        [HttpPost]
        public IActionResult Index([FromBody] CountrySalesAnalyticsRequestModel model)
        {
            var fromDate = model.FromDate;
            var toDate = model.ToDate;

            return this.StatusCode((int)HttpStatusCode.OK);
        }
    }
}
