using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Dashboard.Repositories;
using Seller.Web.Areas.Dashboard.RequestModels;
using Seller.Web.Shared.ApiResponseModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Dashboard.ApiControllers
{
    [Area("Dashboard")]
    public class CountrySalesAnalyticsApiController : BaseApiController
    {
        private readonly ISalesAnalyticsRepository _salesAnalyticsRepository;

        public CountrySalesAnalyticsApiController (
            ISalesAnalyticsRepository salesAnalyticsRepository)
        {
            _salesAnalyticsRepository = salesAnalyticsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SalesAnalyticsRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var countriesSales = await _salesAnalyticsRepository.GetCountriesSales(token, language, model.FromDate, model.ToDate);

            if (countriesSales is not null)
            {
                var response = new ChartResponseModel
                {
                    ChartLabels = countriesSales.Select(x => x.Name),
                    ChartDatasets = new List<ChartDatasetsResponseModel>
                    {
                        new ChartDatasetsResponseModel
                        {
                            Data = countriesSales.Select(x => x.Quantity)
                        }
                    }
                };

                return this.StatusCode((int)HttpStatusCode.OK, new { Data = response });
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
