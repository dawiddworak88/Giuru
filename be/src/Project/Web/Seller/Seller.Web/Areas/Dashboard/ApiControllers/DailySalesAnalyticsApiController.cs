using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Dashboard.Definitions;
using Seller.Web.Areas.Dashboard.Repositories;
using Seller.Web.Areas.Dashboard.RequestModels;
using Seller.Web.Shared.ApiResponseModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Dashboard.ApiControllers
{
    [Area("Dashboard")]
    public class DailySalesAnalyticsApiController : BaseApiController
    {
        private readonly ISalesAnalyticsRepository _salesAnalyticsRepository;

        public DailySalesAnalyticsApiController(ISalesAnalyticsRepository salesAnalyticsRepository)
        {
            _salesAnalyticsRepository = salesAnalyticsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SalesAnalyticsRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var dailySales = await _salesAnalyticsRepository.GetDailySales(token, language, model.FromDate, model.ToDate);

            if (dailySales is not null)
            {
                var chartLabels = new List<string>();

                foreach (var dailySalesItem in dailySales)
                {
                    var monthNumber = dailySalesItem.Month.ToString();

                    if (dailySalesItem.Month < DashboardConstants.MonthNameUnderMonth)
                    {
                        monthNumber = $"0{monthNumber}";
                    }

                    chartLabels.Add($"{dailySalesItem.Day}.{monthNumber}");
                }

                var response = new ChartResponseModel
                {
                    ChartLabels = chartLabels,
                    ChartDatasets = new List<ChartDatasetsResponseModel>
                    {
                        new ChartDatasetsResponseModel
                        {
                            Data = dailySales.Select(x => x.Quantity)
                        }
                    }
                };

                return this.StatusCode((int)HttpStatusCode.OK, new { Data = response });
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
