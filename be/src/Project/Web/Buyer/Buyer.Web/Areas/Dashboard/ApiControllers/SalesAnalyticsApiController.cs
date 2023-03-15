using Buyer.Web.Areas.Dashboard.ApiRequestModels;
using Buyer.Web.Areas.Dashboard.ApiResponseModel;
using Buyer.Web.Areas.Dashboard.Repositories;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.ApiControllers
{
    [Area("Dashboard")]
    public class SalesAnalyticsApiController : BaseApiController
    {
        private readonly ISalesAnalyticsRepository _salesAnalyticsRepository;

        public SalesAnalyticsApiController(
            ISalesAnalyticsRepository salesAnalyticsRepository)
        {
            _salesAnalyticsRepository = salesAnalyticsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SalesAnalyticsRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var annualSales = await _salesAnalyticsRepository.GetAnnualSales(token, language, model.FromDate, model.ToDate);

            if (annualSales is not null)
            {
                var chartLabels = new List<string>();

                foreach (var annualSalesItem in annualSales)
                {
                    var monthName = CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(annualSalesItem.Month);

                    chartLabels.Add($"{monthName.ToUpperInvariant()} - {annualSalesItem.Year}");
                }

                var response = new ChartResponseModel
                {
                    ChartLabels = chartLabels,
                    ChartDatasets = new List<ChartDatasetsResponseModel>
                    {
                        new ChartDatasetsResponseModel
                        {
                            Data = annualSales.Select(x => x.Quantity)
                        }
                    }
                };

                return this.StatusCode((int)HttpStatusCode.OK, new { Data = response });
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
