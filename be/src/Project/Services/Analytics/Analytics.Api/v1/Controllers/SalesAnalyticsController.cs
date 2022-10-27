using Analytics.Api.Services.SalesAnalytics;
using Analytics.Api.ServicesModels.SalesAnalytics;
using Analytics.Api.v1.RequestModels;
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
        private readonly ISalesService salesService;

        public SalesAnalyticsController(
            ISalesService salesService)
        {
            this.salesService = salesService;
        }

        /// <summary>
        /// Gets best selling products
        /// </summary>
        /// <returns>Best selling products</returns>
        [HttpGet("products"), MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string orderBy)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetTopSalesProductsAnalyticsServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                IsSeller = this.User.IsInRole("Seller")
            };

            var validator = new GetTopSalesAnalyticsModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var topSalesProducts = await this.salesService.GetTopSalesProductsAnalyticsAsync(serviceModel);

                if (topSalesProducts is not null)
                {
                    var response = topSalesProducts.Select(x => new TopSalesProductsAnalyticsResponseModel
                    {
                        Id = x.Id,
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

        /// <summary>
        /// Creates  (if inventory skus are set).
        /// </summary>
        /// <param name="request">The list of best selling products.</param>
        /// <returns>OK.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(SalesAnalyticsRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new CreateSalesAnalyticsServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                SalesAnalyticsItems = request.SalesAnalyticsItems.Select(x => new CreateSalesAnalyticsItemServiceModel
                {
                    ClientId = x.ClientId,
                    ClientName = x.ClientName,
                    Email = x.Email,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductSku = x.ProductSku,
                    Ean = x.Ean,
                    IsOutlet = x.IsOutlet,
                    IsStock = x.IsStock,
                    Quantity = x.Quantity,
                    CreatedDate = x.CreatedDate
                })
            };

            var validator = new CreateSalesAnalyticsModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.salesService.CreateSalesAnalyticsAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
