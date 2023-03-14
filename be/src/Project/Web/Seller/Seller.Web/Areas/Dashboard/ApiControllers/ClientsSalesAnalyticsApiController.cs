using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Dashboard.ApiRequestModels;
using Seller.Web.Areas.Dashboard.ApiResponseModels;
using Seller.Web.Areas.Dashboard.Definitions;
using Seller.Web.Areas.Dashboard.Repositories;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Dashboard.ApiControllers
{
    [Area("Dashboard")]
    public class ClientsSalesAnalyticsApiController : BaseApiController
    {
        private readonly ISalesAnalyticsRepository _salesAnalyticsRepository;

        public ClientsSalesAnalyticsApiController(
            ISalesAnalyticsRepository salesAnalyticsRepository)
        {
            _salesAnalyticsRepository = salesAnalyticsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SalesAnalyticsRequestModel request)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var clientsSales = await _salesAnalyticsRepository.GetTopClientsSales(token, language, request.FromDate, request.ToDate, DashboardConstants.DefaultClientsSalesSize);

            if (clientsSales is not null)
            {
                var response = clientsSales.Select(x => new ClientSalesAnalyticsResponseModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Quantity = x.Quantity
                });

                return this.StatusCode((int)HttpStatusCode.OK, new { Data = response });
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
