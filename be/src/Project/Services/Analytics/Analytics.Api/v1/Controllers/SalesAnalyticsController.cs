using Analytics.Api.Services.SalesAnalytics;
using Analytics.Api.ServicesModels.SalesAnalytics;
using Analytics.Api.v1.ResponseModels;
using Analytics.Api.Validators;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Analytics.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class SalesAnalyticsController : BaseApiController
    {
        private readonly ISalesService _salesService;

        public SalesAnalyticsController(
            ISalesService salesService)
        {
            _salesService = salesService;
        }

        /// <summary>
        /// Get annual sales
        /// </summary>
        /// <returns>Annual sales.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetAnnualSales()
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetAnnualSalesServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                IsSeller = User.IsInRole("Seller")
            };

            var validator = new GetAnnualSalesModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var annualSales = _salesService.GetAnnualSalesServiceModel(serviceModel);

                if (annualSales is not null)
                {
                    var response = annualSales.Select(x => new AnnualSalesResponseModel
                    {
                        Month = x.Month,
                        Year = x.Year,
                        Quantity = x.Quantity
                    });

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets best selling products
        /// </summary>
        /// <param name="size">The display limit.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>Best selling products</returns>
        [HttpGet("products"), MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(int? size, string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetTopSalesProductsAnalyticsServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                IsSeller = User.IsInRole("Seller"),
                OrderBy = orderBy,
                Size = size
            };

            var validator = new GetTopSalesAnalyticsModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var topSalesProducts = _salesService.GetTopSalesProductsAnalyticsAsync(serviceModel);

                if (topSalesProducts is not null)
                {
                    var response = topSalesProducts.Select(x => new TopSalesProductsAnalyticsResponseModel
                    {
                        ProductId = x.ProductId,
                        ProductSku = x.ProductSku,
                        ProductName = x.ProductName,
                        Ean = x.Ean,
                        Quantity = x.Quantity
                    });

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
